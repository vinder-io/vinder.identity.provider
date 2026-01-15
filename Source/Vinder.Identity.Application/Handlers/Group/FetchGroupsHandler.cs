namespace Vinder.Identity.Application.Handlers.Group;

public sealed class FetchGroupsHandler(IGroupCollection collection) :
    IRequestHandler<GroupsFetchParameters, Result<Pagination<GroupDetailsScheme>>>
{
    public async Task<Result<Pagination<GroupDetailsScheme>>> Handle(
        GroupsFetchParameters parameters, CancellationToken cancellationToken)
    {
        var filters = GroupMapper.AsFilters(parameters);

        var groups = await collection.GetGroupsAsync(filters, cancellation: cancellationToken);
        var totalGroups = await collection.CountAsync(filters, cancellation: cancellationToken);

        var pagination = new Pagination<GroupDetailsScheme>
        {
            Items = [.. groups.Select(group => GroupMapper.AsResponse(group))],
            Total = (int) totalGroups,
            PageNumber = parameters.Pagination?.PageNumber ?? 1,
            PageSize = parameters.Pagination?.PageSize ?? 20
        };

        return Result<Pagination<GroupDetailsScheme>>.Success(pagination);
    }
}
