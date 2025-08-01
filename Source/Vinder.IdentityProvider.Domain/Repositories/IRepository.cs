namespace Vinder.IdentityProvider.Domain.Repositories;

public interface IRepository<TEntity> where TEntity : Entity
{
    Task<TEntity> InsertAsync(
        TEntity entity,
        CancellationToken cancellation = default
    );

    Task<TEntity> UpdateAsync(
        TEntity entity,
        CancellationToken cancellation = default
    );

    Task<bool> DeleteAsync(
        TEntity entity,
        CancellationToken cancellation = default
    );
}