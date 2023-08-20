using Microsoft.EntityFrameworkCore;
using Pigeon.Contracts;
using Pigeon.Data;

namespace Pigeon.Algoritms;

public class Apriori
{

    public Trainer Trainer { get; private set; }

    private readonly TrainingSet _trainingSet;
    private readonly IServiceScopeFactory _serviceScopeFactory;


    public Apriori(IServiceScopeFactory serviceScopeFactory)
    {
        _trainingSet = new TrainingSet();
        _serviceScopeFactory = serviceScopeFactory;
    }

    public TrainingSet GetTrainingSet()
    {
        return _trainingSet;
    }

    public void SetTrainer(Trainer trainer)
    {
        Trainer = trainer;
    }

    public async Task PrepareModel(CancellationToken cancellationToken)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<PigeonDbContext>();

            var tags = await dbContext.Tags.AsNoTracking().ToListAsync(cancellationToken);

            var samples = tags.GroupBy(s => s.UrlId).Select(s => new TrainingSample(s.Key, s.Select(s => s.Title).ToList())).ToList();

            _trainingSet.AddSample(samples);

            _trainingSet.Lock();
        }
    }
}
