namespace Vinder.IdentityProvider.Application.Handlers.Group;

public sealed class GroupDeletionHandler(IGroupRepository repository) : IRequestHandler<GroupForDeletion, Result>
{
    public async Task<Result> Handle(GroupForDeletion request, CancellationToken cancellationToken)
    {
        var filters = new GroupFiltersBuilder()
            .WithId(request.GroupId)
            .Build();

        var groups = await repository.GetGroupsAsync(filters, cancellation: cancellationToken);
        var group = groups.FirstOrDefault();

        if (group is null)
        {
            return Result.Failure(GroupErrors.GroupDoesNotExist);
        }

        await repository.DeleteAsync(group, cancellation: cancellationToken);

        return Result.Success();
    }
}