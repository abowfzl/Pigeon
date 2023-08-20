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

    [BindProperty]
    public string Input { get; set; }

    public async Task<IActionResult> OnGet(CancellationToken cancellationToken)
    {
        // var trainerSet = _apriori.GetTrainingSet();

        // if (!trainerSet.Samples.Any())
        //     await _apriori.PrepareModel(cancellationToken);

        // var trainer = new Trainer(trainerSet, 60, 75, _logger);

        // _apriori.SetTrainer(trainer);

        // _apriori.Trainer.Train();

        return Page();
    }

    public IActionResult OnPost()
    {
        // var inputs = Input.Split(",").Select(s => s.Trim());
        
        // var listOFList = new List<string[]>();
        
        // foreach (var input in inputs)
        // {
        //     var inputMatched = _apriori.Trainer.Results.Where(s => s.Key.Contains(input)).OrderBy(s => s.Value.Length).FirstOrDefault().Value;

        //     listOFList.Add(inputMatched);
        // }

        var response = new List<string>(){ "اصغرر"};
        
        // foreach (var input in listOFList.OrderByDescending(s => s.Length))
        // {
        //     if (!response.Any())
        //         response.AddRange(input);

        //     response = response.Intersect(input).ToList();
        // }

        return Content(JsonSerializer.Serialize(response), "application/json");
    }
}
