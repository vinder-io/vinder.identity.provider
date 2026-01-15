namespace Vinder.Identity.Domain.Repositories;

public interface IUserCollection : IAggregateCollection<User>
{
    public Task<IReadOnlyCollection<User>> GetUsersAsync(
        UserFilters filters,
        CancellationToken cancellation = default
    );

    public Task<long> CountAsync(
        UserFilters filters,
        CancellationToken cancellation = default
    );
}