namespace Vinder.Federation.Application.Handlers.OpenID;

public sealed class FetchOpenIDConfigurationHandler(IHostInformationProvider host) :
    IMessageHandler<FetchOpenIDConfigurationParameters, Result<OpenIDConfigurationScheme>>
{
    public Task<Result<OpenIDConfigurationScheme>> HandleAsync(
        FetchOpenIDConfigurationParameters parameters, CancellationToken cancellation = default)
    {
        var configuration = OpenIDMapper.AsConfiguration(host.Address);

        return Task.FromResult(Result<OpenIDConfigurationScheme>.Success(configuration));
    }
}
