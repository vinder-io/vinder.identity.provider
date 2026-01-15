namespace Vinder.Identity.Domain.Repositories;

public interface IGroupRepository : IBaseRepository<Group>
{
    public Task<IReadOnlyCollection<Group>> GetGroupsAsync(
        GroupFilters filters,
        CancellationToken cancellation = default
    );

    public Task<long> CountAsync(
        GroupFilters filters,
        CancellationToken cancellation = default
    );
}