namespace Vinder.IdentityProvider.Application.Handlers.Group;

public sealed class GroupUpdateHandler(IGroupRepository repository) :
    IRequestHandler<GroupUpdateScheme, Result<GroupDetailsScheme>>
{
    public async Task<Result<GroupDetailsScheme>> Handle(
        GroupUpdateScheme request, CancellationToken cancellationToken)
    {
        var filters = new GroupFiltersBuilder()
            .WithId(request.GroupId)
            .Build();

        var groups = await repository.GetGroupsAsync(filters, cancellation: cancellationToken);
        var group = groups.FirstOrDefault();

        if (group is null)
        {
            return Result<GroupDetailsScheme>.Failure(GroupErrors.GroupDoesNotExist);
        }

        group = GroupMapper.AsGroup(request, group);

        var updatedGroup = await repository.UpdateAsync(group, cancellation: cancellationToken);

        return Result<GroupDetailsScheme>.Success(GroupMapper.AsResponse(updatedGroup));
    }
}
