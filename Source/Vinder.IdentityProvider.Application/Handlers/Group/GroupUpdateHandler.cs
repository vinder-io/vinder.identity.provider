namespace Vinder.IdentityProvider.Application.Handlers.Group;

public sealed class GroupUpdateHandler(IGroupRepository repository) :
    IRequestHandler<GroupForUpdate, Result<GroupDetails>>
{
    public async Task<Result<GroupDetails>> Handle(GroupForUpdate request, CancellationToken cancellationToken)
    {
        var filters = new GroupFiltersBuilder()
            .WithId(request.GroupId)
            .Build();

        var groups = await repository.GetGroupsAsync(filters, cancellation: cancellationToken);
        var group = groups.FirstOrDefault();

        if (group is null)
        {
            return Result<GroupDetails>.Failure(GroupErrors.GroupDoesNotExist);
        }

        group = GroupMapper.AsGroup(request, group);

        var updatedGroup = await repository.UpdateAsync(group, cancellation: cancellationToken);

        return Result<GroupDetails>.Success(GroupMapper.AsResponse(updatedGroup));
    }
}
