namespace Vinder.IdentityProvider.Infrastructure.Pipelines;

public static class PageFiltersStage
{
    public static PipelineDefinition<TInput, TOutput> Paginate<TInput, TOutput>(
        this PipelineDefinition<TInput, TOutput> pipeline,
        PaginationFilters filters)
    {
        return pipeline
            .Skip(filters.Skip)
            .Limit(filters.Take);
    }
}