namespace Vinder.IdentityProvider.Domain.Repositories;

public interface IGroupRepository : IRepository<Group>
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