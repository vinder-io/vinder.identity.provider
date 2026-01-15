namespace Vinder.Identity.Domain.Repositories;

public interface ITokenRepository : IAggregateCollection<SecurityToken>
{
    public Task<IReadOnlyCollection<SecurityToken>> GetTokensAsync(
        TokenFilters filters,
        CancellationToken cancellation = default
    );

    public Task<long> CountAsync(
        TokenFilters filters,
        CancellationToken cancellation = default
    );
}