using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pigeon.Algoritms;
using System.Text.Json;

namespace Pigeon.Pages;

public class DataMiningModel : PageModel
{
    private readonly Apriori _apriori;
    private readonly ILogger<DataMiningModel> _logger;

    public DataMiningModel(Apriori apriori,
        ILogger<DataMiningModel> logger)
    {
        _apriori = apriori;
        _logger = logger;
    }

    public class DataMiningInput
    {
        public string Input { get; set; } = string.Empty;
    }

    public async Task<IActionResult> OnGet(CancellationToken cancellationToken)
    {
        var trainerSet = _apriori.GetTrainingSet();

        if (!trainerSet.Samples.Any())
        {
            await _apriori.PrepareModel(cancellationToken);

            var trainer = new Trainer(trainerSet, 60, 75, _logger);

            _apriori.SetTrainer(trainer);

            _apriori.Trainer.Train();
        }

        return Page();
    }

    public IActionResult OnPost([FromBody] DataMiningInput Input)
    {
        var response = new List<string>();

        if (Input != null && !string.IsNullOrEmpty(Input.Input))
        {
            var inputs = Input.Input.Split(",").Select(s => s.Trim());

            var listOFList = new List<List<string>>();

            foreach (var input in inputs)
            {
                var inputMatched = _apriori.Trainer.Results.OrderBy(s => s.Ins.Count).Where(s => s.Ins.Contains(input)).FirstOrDefault()?.Outs.ToList();

                if (inputMatched is not null)
                    listOFList.Add(inputMatched);
            }

            foreach (var input in listOFList.OrderByDescending(s => s.Count))
            {
                if (!response.Any())
                    response.AddRange(input);

                response = response.Intersect(input).ToList();
            }
        }

        return Content(JsonSerializer.Serialize(response.FirstOrDefault()), "application/json");
    }
}
