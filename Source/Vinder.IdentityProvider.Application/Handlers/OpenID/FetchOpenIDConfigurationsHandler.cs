namespace Vinder.IdentityProvider.Application.Handlers.OpenID;

public sealed class FetchOpenIDConfigurationHandler(IHostInformationProvider host) :
    IRequestHandler<FetchOpenIDConfigurationRequest, Result<OpenIDConfiguration>>
{
    public Task<Result<OpenIDConfiguration>> Handle(
        FetchOpenIDConfigurationRequest request, CancellationToken cancellationToken)
    {
        var configuration = OpenIDMapper.AsConfiguration(host.Address);

        return Task.FromResult(Result<OpenIDConfiguration>.Success(configuration));
    }
}
