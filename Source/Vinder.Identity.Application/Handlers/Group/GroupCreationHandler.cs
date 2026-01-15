namespace Vinder.Identity.Application.Handlers.Group;

public sealed class GroupCreationHandler(IGroupRepository groupRepository, ITenantProvider tenantProvider) :
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

        var groups = await groupRepository.GetGroupsAsync(filters, cancellation: cancellationToken);
        var existingGroup = groups.FirstOrDefault();

        if (existingGroup is not null)
        {
            return Result<GroupDetailsScheme>.Failure(GroupErrors.GroupAlreadyExists);
        }

        await groupRepository.InsertAsync(group, cancellation: cancellationToken);

        return Result<GroupDetailsScheme>.Success(GroupMapper.AsResponse(group));
    }
}