namespace Vinder.IdentityProvider.Application.Handlers.User;

public sealed class FetchUsersHandler(IUserRepository repository) :
    IRequestHandler<UsersFetchParameters, Result<Pagination<UserDetails>>>
{
    public async Task<Result<Pagination<UserDetails>>> Handle(UsersFetchParameters request, CancellationToken cancellationToken)
    {
        var filters = UserMapper.AsFilters(request);

        var matchingUsers = await repository.GetUsersAsync(filters, cancellationToken);
        var totalUsers = await repository.CountAsync(filters, cancellationToken);

        var users = matchingUsers.Select(user =>
        {
            var unifiedPermissions = user.Permissions
                .Concat(user.Groups.SelectMany(group => group.Permissions))
                .DistinctBy(permission => permission.Id)
                .Select(PermissionMapper.AsResponse)
                .ToArray();

            var payload = UserMapper.AsResponse(user) with
            {
                Permissions = unifiedPermissions
            };

            return payload;
        });

        var pagination = new Pagination<UserDetails>
        {
            Items = [.. users],
            Total = (int) totalUsers,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
        };

        return Result<Pagination<UserDetails>>.Success(pagination);
    }
}
