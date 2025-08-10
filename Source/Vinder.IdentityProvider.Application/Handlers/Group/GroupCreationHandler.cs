namespace Vinder.IdentityProvider.Application.Handlers.Group;

public sealed class GroupCreationHandler(IGroupRepository groupRepository, ITenantProvider tenantProvider) :
    IRequestHandler<GroupForCreation, Result<GroupDetails>>
{
    public async Task<Result<GroupDetails>> Handle(GroupForCreation request, CancellationToken cancellationToken)
    {
        var tenant = tenantProvider.GetCurrentTenant();
        var group = GroupMapper.AsGroup(request, tenant);

        await groupRepository.InsertAsync(group, cancellation: cancellationToken);

        return Result<GroupDetails>.Success(GroupMapper.AsResponse(group));
    }
}