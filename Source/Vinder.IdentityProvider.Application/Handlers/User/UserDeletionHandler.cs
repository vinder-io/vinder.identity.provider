namespace Vinder.IdentityProvider.Application.Handlers.User;

public sealed class UserDeletionHandler(IUserRepository repository) : IRequestHandler<UserDeletionScheme, Result>
{
    public async Task<Result> Handle(UserDeletionScheme request, CancellationToken cancellationToken)
    {
        var filters = new UserFiltersBuilder()
            .WithUserId(request.UserId)
            .Build();

        var users = await repository.GetUsersAsync(filters, cancellation: cancellationToken);
        var user = users.FirstOrDefault();

        if (user is null)
        {
            return Result.Failure(UserErrors.UserDoesNotExist);
        }

        await repository.DeleteAsync(user, cancellation: cancellationToken);

        return Result.Success();
    }
}