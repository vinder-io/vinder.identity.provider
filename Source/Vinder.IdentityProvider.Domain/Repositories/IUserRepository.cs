namespace Vinder.IdentityProvider.Domain.Repositories;

public interface IUserRepository : IBaseRepository<User>
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