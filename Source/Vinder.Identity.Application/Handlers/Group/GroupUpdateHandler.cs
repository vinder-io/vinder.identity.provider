namespace Vinder.Identity.Application.Handlers.Group;

public sealed class GroupUpdateHandler(IGroupCollection collection) :
    IRequestHandler<GroupUpdateScheme, Result<GroupDetailsScheme>>
{
    public async Task<Result<GroupDetailsScheme>> Handle(
        GroupUpdateScheme request, CancellationToken cancellationToken)
    {
        var filters = new GroupFiltersBuilder()
            .WithIdentifier(request.GroupId)
            .Build();

        var groups = await collection.GetGroupsAsync(filters, cancellation: cancellationToken);
        var group = groups.FirstOrDefault();

        if (group is null)
        {
            return Result<GroupDetailsScheme>.Failure(GroupErrors.GroupDoesNotExist);
        }

        group = GroupMapper.AsGroup(request, group);

        var updatedGroup = await collection.UpdateAsync(group, cancellation: cancellationToken);

        return Result<GroupDetailsScheme>.Success(GroupMapper.AsResponse(updatedGroup));
    }
}
