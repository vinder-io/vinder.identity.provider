namespace Vinder.IdentityProvider.Application.Handlers.OpenID;

public sealed class FetchOpenIDConfigurationHandler(IHostInformationProvider host) :
    IRequestHandler<FetchOpenIDConfigurationParameters, Result<OpenIDConfigurationScheme>>
{
    public Task<Result<OpenIDConfigurationScheme>> Handle(
        FetchOpenIDConfigurationParameters request, CancellationToken cancellationToken)
    {
        var configuration = OpenIDMapper.AsConfiguration(host.Address);

        return Task.FromResult(Result<OpenIDConfigurationScheme>.Success(configuration));
    }
}
