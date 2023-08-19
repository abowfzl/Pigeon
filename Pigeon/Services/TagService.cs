using Pigeon.Data;
using Pigeon.Entities;

namespace Pigeon.Services
{
    public class TagService
    {
        private readonly PigeonDbContext _dbContext;

        public TagService(PigeonDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task InsertTags(IList<Tag> tags, CancellationToken cancellationToken)
        {
            tags = tags.DistinctBy(t => t.Title).ToList();

            await _dbContext.Tags.AddRangeAsync(tags, cancellationToken);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

    }
}
