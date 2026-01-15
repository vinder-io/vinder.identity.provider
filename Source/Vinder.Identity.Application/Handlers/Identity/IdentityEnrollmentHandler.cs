namespace Vinder.Identity.Application.Handlers.Identity;

public sealed class IdentityEnrollmentHandler(
    IUserCollection userCollection,
    IPasswordHasher passwordHasher,
    ITenantProvider tenantProvider
) : IRequestHandler<IdentityEnrollmentCredentials, Result<UserDetailsScheme>>
{
    public async Task<Result<UserDetailsScheme>> Handle(IdentityEnrollmentCredentials request, CancellationToken cancellationToken)
    {
        var filters = new UserFiltersBuilder()
            .WithUsername(request.Username)
            .Build();

        var users = await userCollection.GetUsersAsync(filters, cancellationToken);
        var user = users.FirstOrDefault();

        if (user is not null)
        {
            return Result<UserDetailsScheme>.Failure(IdentityErrors.UserAlreadyExists);
        }

        var tenant = tenantProvider.GetCurrentTenant();
        var identity = UserMapper.AsUser(request, tenant.Id);

        identity.PasswordHash = await passwordHasher.HashPasswordAsync(request.Password);

        await userCollection.InsertAsync(identity, cancellation: cancellationToken);

        return Result<UserDetailsScheme>.Success(UserMapper.AsResponse(identity));
    }
}