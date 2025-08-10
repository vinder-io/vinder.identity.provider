using Vinder.IdentityProvider.Application.Providers;
using Vinder.IdentityProvider.Common.Errors;
using Vinder.IdentityProvider.Domain.Filters.Builders;
using Vinder.IdentityProvider.Domain.Repositories;

namespace Vinder.IdentityProvider.Application.Handlers.Identity;

public sealed class IdentityEnrollmentHandler(
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    ITenantProvider tenantProvider
) : IRequestHandler<IdentityEnrollmentCredentials, Result>
{
    public async Task<Result> Handle(IdentityEnrollmentCredentials request, CancellationToken cancellationToken)
    {
        var filters = new UserFiltersBuilder()
            .WithUsername(request.Username)
            .Build();

        var users = await userRepository.GetUsersAsync(filters, cancellationToken);
        var user = users.FirstOrDefault();

        if (user is not null)
        {
            return Result.Failure(IdentityErrors.UserAlreadyExists);
        }

        var tenant = tenantProvider.GetCurrentTenant();
        var identity = new User
        {
            Username = request.Username,
            TenantId = tenant.Id,
            PasswordHash = await passwordHasher.HashPasswordAsync(request.Password)
        };

        await userRepository.InsertAsync(identity, cancellationToken);

        return Result.Success();
    }
}