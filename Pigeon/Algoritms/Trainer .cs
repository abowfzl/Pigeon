﻿namespace Pigeon.Algoritms;

public sealed class Trainer
{
    private TrainingSet m_set;

    private int m_support;
    private int m_confidence;

    private int m_thresholdSupport;

    private readonly ILogger _logger;

    public IList<TrainerResult> Results { get; private set; }

    public Trainer(TrainingSet set, int support, int confidence, ILogger logger)
    {
        m_set = set;
        m_support = support > 100 ? 100 : (support < 0 ? 0 : support);
        m_confidence = confidence > 100 ? 100 : (confidence < 0 ? 0 : confidence);

        m_thresholdSupport = m_set.Samples.Count * m_support / 100;

        _logger = logger;

        Results = new List<TrainerResult>();
    }

    public void Train()
    {
        Dictionary<string, int> productCounts = CalculateProductCounts();

        _logger.LogInformation($"support(threshold) = %{m_support}");
        _logger.LogInformation($"trust(threshold)  = %{m_confidence}");

        productCounts = RemoveThreshold(productCounts);

        Dictionary<string[], int> grouped = GroupProducts(productCounts);

        grouped = RemoveThreshold(grouped);

        grouped = MergeGroupProducts(grouped);

        PrintFinalValues(grouped);
    }

    private void PrintFinalValues(Dictionary<string[], int> group)
    {

        _logger.LogInformation("RESULTS");
        _logger.LogInformation("--------------------");
        int index = 1;
        foreach (KeyValuePair<string[], int> product in group)
        {
            string[] keys = product.Key;

            for (int i = -1; i < keys.Length; i++)
            {
                _logger.LogInformation($"RESULT {index} : ");

                string[] ins;
                string[] outs;

                if (i == -1)
                {
                    ins = new string[] { keys[0], keys[1] };
                    outs = keys.Except(ins).ToArray();
                    PrintThresholdRule(keys, ins, outs);
                    index++;
                    continue;
                }

                ins = new string[] { keys[i] };
                outs = keys.Except(ins).ToArray();
                if (i < outs.Length)
                {
                    var tmp = outs[0];
                    var tmp2 = outs[i];
                    outs[0] = tmp2;
                    outs[i] = tmp;
                }

                PrintThresholdRule(keys, ins, outs);

                index++;
            }
        }
        _logger.LogInformation("---------------------------------------------------------------------------------------------");
    }

    private Dictionary<string[], int> MergeGroupProducts(Dictionary<string[], int> grouped)
    {
        Dictionary<string[], int> temp = new(new ArrayComparer());
        List<string> datas = new();
        foreach (KeyValuePair<string[], int> main in grouped)
        {
            string[] keys = main.Key;
            for (int i = 0; i < keys.Length; i++)
            {
                if (!datas.Contains(keys[i]))
                {
                    datas.Add(keys[i]);
                }
            }
        }
        temp.Add(datas.ToArray(), GetGroupCountInSamples(datas.ToArray()));
        return temp;
    }

    private Dictionary<string[], int> GroupProducts(Dictionary<string, int> productCounts)
    {
        Dictionary<string[], int> temp = new(new ArrayComparer());
        foreach (KeyValuePair<string, int> main in productCounts)
        {
            string mainKey = main.Key;
            foreach (KeyValuePair<string, int> sub in productCounts)
            {
                string subKey = sub.Key;
                if (!mainKey.Equals(subKey))
                {
                    string[] head1 = new string[] { mainKey, subKey };
                    string[] head2 = head1.Reverse().ToArray();
                    if (!temp.ContainsKey(head1) && !temp.ContainsKey(head2))
                    {
                        temp.Add(head1, GetGroupCountInSamples(head1));
                    }
                }
            }
        }
        return temp;
    }

    private void PrintThresholdRule(string[] keys, string[] ins, string[] outs)
    {
        int XYZ = GetGroupCountInSamples(keys);
        int N = GetGroupCountInSamples(ins);
        double result = (double)XYZ / (double)N * 100;

        var trainerResult = new TrainerResult()
        {
            Ins = ins,
            Outs = outs,
            Percentage = 100
        };
        Results.Add(trainerResult);
    }

    private int GetGroupCountInSamples(string[] head)
    {
        int temp = 0;
        for (int i = 0; i < m_set.Samples.Count; i++)
        {
            if (head.Except(m_set.Samples[i].Products).Count() == 0)
            {
                temp++;
            }
        }
        return temp;
    }

    private static Dictionary<string, int> RemoveThreshold(Dictionary<string, int> productCounts)
    {
        return productCounts.Where(pair => pair.Value >= 3).ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    private static Dictionary<string[], int> RemoveThreshold(Dictionary<string[], int> productCounts)
    {
        return productCounts.Where(pair => pair.Value >= 2).ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    private Dictionary<string, int> CalculateProductCounts()
    {
        Dictionary<string, int> temp = new();
        for (int i = 0; i < m_set.Samples.Count; i++)
        {
            for (int j = 0; j < m_set.Samples[i].Products.Count; j++)
            {
                string product = m_set.Samples[i].Products[j];
                if (temp.ContainsKey(product))
                {
                    temp[product]++;
                }
                else
                {
                    temp.Add(product, 1);
                }
            }
        }
        return temp;
    }
}
