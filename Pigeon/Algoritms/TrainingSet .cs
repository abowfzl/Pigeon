namespace Pigeon.Algoritms;

public sealed class TrainingSet
{
    public List<TrainingSample> Samples { get; private set; }

    private bool m_canAddSample = true;

    public TrainingSet()
    {
        Samples = new List<TrainingSample>();
    }

    public void AddSample(IList<TrainingSample> samples)
    {
        if (m_canAddSample)
        {
            foreach (TrainingSample sample in samples)
            {
                if (!Samples.Contains(sample))
                {
                    Samples.Add(sample);
                }
                else
                {
                    throw new Exception("Already contains value");
                }
            }
        }
        else
        {
            throw new Exception("Training set was locked");
        }
    }

    public void Lock()
    {
        m_canAddSample = false;
    }
}