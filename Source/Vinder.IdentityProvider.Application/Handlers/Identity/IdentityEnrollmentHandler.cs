namespace Vinder.IdentityProvider.Application.Handlers.Identity;

public sealed class IdentityEnrollmentHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITenantProvider tenantProvider
) : IRequestHandler<IdentityEnrollmentCredentials, Result<UserDetails>>
{
    public async Task<Result<UserDetails>> Handle(IdentityEnrollmentCredentials request, CancellationToken cancellationToken)
    {
        var filters = new UserFiltersBuilder()
            .WithUsername(request.Username)
            .Build();

        var users = await userRepository.GetUsersAsync(filters, cancellationToken);
        var user = users.FirstOrDefault();

        if (user is not null)
        {
            return Result<UserDetails>.Failure(IdentityErrors.UserAlreadyExists);
        }

        var tenant = tenantProvider.GetCurrentTenant();
        var identity = UserMapper.AsUser(request, tenant.Id);

        identity.PasswordHash = await passwordHasher.HashPasswordAsync(request.Password);

        await userRepository.InsertAsync(identity, cancellationToken);

        return Result<UserDetails>.Success(UserMapper.AsResponse(identity));
    }
}