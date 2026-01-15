namespace Vinder.Identity.Application.Handlers.Group;

public sealed class GroupCreationHandler(IGroupCollection groupCollection, ITenantProvider tenantProvider) :
    IRequestHandler<GroupCreationScheme, Result<GroupDetailsScheme>>
{
    public async Task<Result<GroupDetailsScheme>> Handle(
        GroupCreationScheme request, CancellationToken cancellationToken)
    {
        var tenant = tenantProvider.GetCurrentTenant();
        var group = GroupMapper.AsGroup(request, tenant);

        var filters = new GroupFiltersBuilder()
            .WithName(group.Name)
            .Build();

        var groups = await groupCollection.GetGroupsAsync(filters, cancellation: cancellationToken);
        var existingGroup = groups.FirstOrDefault();

        if (existingGroup is not null)
        {
            return Result<GroupDetailsScheme>.Failure(GroupErrors.GroupAlreadyExists);
        }

        await groupCollection.InsertAsync(group, cancellation: cancellationToken);

        return Result<GroupDetailsScheme>.Success(GroupMapper.AsResponse(group));
    }
}