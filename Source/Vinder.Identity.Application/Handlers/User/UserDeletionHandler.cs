namespace Vinder.Identity.Application.Handlers.User;

public sealed class UserDeletionHandler(IUserCollection collection) : IRequestHandler<UserDeletionScheme, Result>
{
    public async Task<Result> Handle(UserDeletionScheme request, CancellationToken cancellationToken)
    {
        var filters = new UserFiltersBuilder()
            .WithIdentifier(request.UserId)
            .Build();

        var users = await collection.GetUsersAsync(filters, cancellation: cancellationToken);
        var user = users.FirstOrDefault();

        if (user is null)
        {
            return Result.Failure(UserErrors.UserDoesNotExist);
        }

        await collection.DeleteAsync(user, cancellation: cancellationToken);

        return Result.Success();
    }
}