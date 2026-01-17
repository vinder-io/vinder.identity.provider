namespace Vinder.Identity.Application.Handlers.Group;

public sealed class GroupCreationHandler(IGroupCollection groupCollection, ITenantProvider tenantProvider) :
    IMessageHandler<GroupCreationScheme, Result<GroupDetailsScheme>>
{
    public async Task<Result<GroupDetailsScheme>> HandleAsync(
        GroupCreationScheme parameters, CancellationToken cancellation = default)
    {
        var tenant = tenantProvider.GetCurrentTenant();
        var group = GroupMapper.AsGroup(parameters, tenant);

        var filters = new GroupFiltersBuilder()
            .WithName(group.Name)
            .Build();

        var groups = await groupCollection.GetGroupsAsync(filters, cancellation: cancellation);
        var existingGroup = groups.FirstOrDefault();

        if (existingGroup is not null)
        {
            return Result<GroupDetailsScheme>.Failure(GroupErrors.GroupAlreadyExists);
        }

        await groupCollection.InsertAsync(group, cancellation: cancellation);

        return Result<GroupDetailsScheme>.Success(GroupMapper.AsResponse(group));
    }
}