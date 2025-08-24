namespace Vinder.IdentityProvider.Application.Handlers.User;

public sealed class FetchUsersHandler(IUserRepository repository) :
    IRequestHandler<UsersFetchParameters, Result<Pagination<UserDetails>>>
{
    public async Task<Result<Pagination<UserDetails>>> Handle(UsersFetchParameters request, CancellationToken cancellationToken)
    {
        var filters = UserMapper.AsFilters(request);

        var users = await repository.GetUsersAsync(filters, cancellationToken);
        var totalUsers = await repository.CountAsync(filters, cancellationToken);

        var pagination = new Pagination<UserDetails>
        {
            Items = [.. users.Select(user => UserMapper.AsResponse(user))],
            Total = (int) totalUsers,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
        };

        return Result<Pagination<UserDetails>>.Success(pagination);
    }
}
