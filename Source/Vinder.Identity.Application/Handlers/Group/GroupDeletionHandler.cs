namespace Vinder.Identity.Application.Handlers.Group;

public sealed class GroupDeletionHandler(IGroupCollection collection) : IRequestHandler<GroupDeletionScheme, Result>
{
    public async Task<Result> Handle(GroupDeletionScheme request, CancellationToken cancellationToken)
    {
        var filters = new GroupFiltersBuilder()
            .WithIdentifier(request.GroupId)
            .Build();

        var groups = await collection.GetGroupsAsync(filters, cancellation: cancellationToken);
        var group = groups.FirstOrDefault();

        if (group is null)
        {
            return Result.Failure(GroupErrors.GroupDoesNotExist);
        }

        await collection.DeleteAsync(group, cancellation: cancellationToken);

        return Result.Success();
    }
}