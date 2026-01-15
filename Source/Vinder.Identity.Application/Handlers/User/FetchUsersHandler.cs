namespace Vinder.Identity.Application.Handlers.User;

public sealed class FetchUsersHandler(IUserCollection collection) :
    IRequestHandler<UsersFetchParameters, Result<Pagination<UserDetailsScheme>>>
{
    public async Task<Result<Pagination<UserDetailsScheme>>> Handle(
        UsersFetchParameters parameters, CancellationToken cancellationToken)
    {
        var filters = UserMapper.AsFilters(parameters);

        var users = await collection.GetUsersAsync(filters, cancellationToken);
        var totalUsers = await collection.CountAsync(filters, cancellationToken);

        var pagination = new Pagination<UserDetailsScheme>
        {
            Items = [.. users.Select(user => UserMapper.AsResponse(user))],
            Total = (int) totalUsers,
            PageNumber = parameters.Pagination?.PageNumber ?? 1,
            PageSize = parameters.Pagination?.PageSize ?? 20,
        };

        return Result<Pagination<UserDetailsScheme>>.Success(pagination);
    }
}
