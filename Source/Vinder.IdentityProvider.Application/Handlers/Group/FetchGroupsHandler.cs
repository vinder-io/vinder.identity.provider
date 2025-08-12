namespace Vinder.IdentityProvider.Application.Handlers.Group;

public sealed class FetchGroupsHandler(IGroupRepository repository) :
    IRequestHandler<GroupsFetchParameters, Result<Pagination<GroupDetails>>>
{
    public async Task<Result<Pagination<GroupDetails>>> Handle(GroupsFetchParameters request, CancellationToken cancellationToken)
    {
        var filters = GroupMapper.AsFilters(request);

        var groups = await repository.GetGroupsAsync(filters, cancellation: cancellationToken);
        var totalGroups = await repository.CountAsync(filters, cancellation: cancellationToken);

        var pagination = new Pagination<GroupDetails>
        {
            Items = [.. groups.Select(group => GroupMapper.AsResponse(group))],
            Total = (int) totalGroups,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
        };

        return Result<Pagination<GroupDetails>>.Success(pagination);
    }
}
