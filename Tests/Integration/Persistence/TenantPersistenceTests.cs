namespace Vinder.Identity.TestSuite.Integration.Persistence;

public sealed class TenantPersistenceTests : IClassFixture<MongoDatabaseFixture>, IAsyncLifetime
{
    private readonly ITenantCollection _tenantCollection;
    private readonly IMongoDatabase _database;
    private readonly MongoDatabaseFixture _mongoFixture;
    private readonly Fixture _fixture = new();

    public TenantPersistenceTests(MongoDatabaseFixture fixture)
    {
        _mongoFixture = fixture;
        _database = fixture.Database;
        _tenantCollection = new TenantCollection(_database);
    }

    [Fact(DisplayName = "[infrastructure] - when inserting a tenant, then it must persist in the database")]
    public async Task WhenInsertingATenant_ThenItMustPersistInTheDatabase()
    {
        /* arrange: create tenant and matching filter */
        var tenant = _fixture.Build<Tenant>()
            .With(tenant => tenant.Name, "Vinder")
            .With(tenant => tenant.IsDeleted, false)
            .Create();

        var filters = TenantFilters.WithSpecifications()
            .WithName(tenant.Name)
            .Build();

        /* act: persist tenant and query using name filter */
        await _tenantCollection.InsertAsync(tenant);

        var result = await _tenantCollection.GetTenantsAsync(filters, CancellationToken.None);
        var retrievedTenant = result.FirstOrDefault();

        /* assert: tenant must be retrieved with same id and name */
        Assert.NotNull(retrievedTenant);
        Assert.Equal(tenant.Id, retrievedTenant.Id);
        Assert.Equal(tenant.Name, retrievedTenant.Name);
    }

    [Fact(DisplayName = "[infrastructure] - when updating a tenant, then updated fields must persist")]
    public async Task WhenUpdatingATenant_ThenUpdatedFieldsMustPersist()
    {
        /* arrange: create and insert tenant */
        var tenant = _fixture.Build<Tenant>()
            .With(tenant => tenant.Name, "update.test")
            .With(tenant => tenant.IsDeleted, false)
            .Create();

        await _tenantCollection.InsertAsync(tenant);

        /* act: update name and save */
        var newName = "updated.name";

        tenant.Name = newName;

        await _tenantCollection.UpdateAsync(tenant);

        var filters = TenantFilters.WithSpecifications()
            .WithName(newName)
            .Build();

        var result = await _tenantCollection.GetTenantsAsync(filters, CancellationToken.None);
        var updatedTenant = result.FirstOrDefault();

        /* assert: updated tenant must be found with new name */
        Assert.NotNull(updatedTenant);

        Assert.Equal(tenant.Id, updatedTenant.Id);
        Assert.Equal(newName, updatedTenant.Name);
    }

    [Fact(DisplayName = "[infrastructure] - when deleting a tenant, then it must be marked as deleted and not returned by filters")]
    public async Task WhenDeletingATenant_ThenItMustBeMarkedDeletedAndExcludedFromResults()
    {
        /* arrange: create and insert tenant */
        var tenant = _fixture.Build<Tenant>()
            .With(tenant => tenant.Name, "delete.test")
            .With(tenant => tenant.IsDeleted, false)
            .Create();

        await _tenantCollection.InsertAsync(tenant);

        var filters = TenantFilters.WithSpecifications()
            .WithName(tenant.Name)
            .Build();

        /* act: delete tenant and query by name */
        var deleted = await _tenantCollection.DeleteAsync(tenant);

        var resultAfterDelete = await _tenantCollection.GetTenantsAsync(filters, CancellationToken.None);

        /* assert: no tenants should be returned after delete */
        Assert.DoesNotContain(resultAfterDelete, t => t.Id == tenant.Id);

        /* arrange: prepare filters including deleted tenants */
        var filtersWithDeleted = TenantFilters.WithSpecifications()
            .WithName(tenant.Name)
            .WithIsDeleted(true)
            .Build();

        /* act: refetch tenants including deleted */
        var resultWithDeleted = await _tenantCollection.GetTenantsAsync(filtersWithDeleted, CancellationToken.None);

        /* assert: tenant should be returned when including deleted tenants */
        Assert.Contains(resultWithDeleted, t => t.Id == tenant.Id);

        Assert.True(tenant.IsDeleted);
        Assert.True(deleted);
    }

    [Fact(DisplayName = "[infrastructure] - when filtering tenants, then it must return matching tenants")]
    public async Task WhenFilteringTenants_ThenItMustReturnOnlyMatchingTenants()
    {
        /* arrange: insert two tenants with different names */
        var tenant1 = _fixture.Build<Tenant>()
            .With(tenant => tenant.Name, "filter1")
            .With(tenant => tenant.IsDeleted, false)
            .Create();

        var tenant2 = _fixture.Build<Tenant>()
            .With(tenant => tenant.Name, "filter2")
            .With(tenant => tenant.IsDeleted, false)
            .Create();

        await _tenantCollection.InsertAsync(tenant1);
        await _tenantCollection.InsertAsync(tenant2);

        var filters = TenantFilters.WithSpecifications()
            .WithName("filter1")
            .Build();

        /* act: query tenants filtered by name */
        var filteredTenants = await _tenantCollection.GetTenantsAsync(filters, CancellationToken.None);

        /* assert: only tenant1 should be returned */
        Assert.Single(filteredTenants);
        Assert.Equal(tenant1.Id, filteredTenants.First().Id);
    }

    [Fact(DisplayName = "[infrastructure] - when paginating 10 tenants with page size 5, then it must return 5 tenants per page")]
    public async Task WhenPaginatingTenTenants_ThenItMustReturnFiveTenantsPerPage()
    {
        /* arrange: create and insert 10 tenants, all not deleted */
        var tenants = Enumerable.Range(1, 10)
            .Select(index => _fixture.Build<Tenant>()
            .With(tenant => tenant.Name, $"tenant.{index}")
            .With(tenant => tenant.IsDeleted, false)
            .Create())
            .ToList();

        foreach (var tenant in tenants)
        {
            await _tenantCollection.InsertAsync(tenant);
        }

        /* arrange: prepare filters for page 1 with page size 5 */
        var filtersPage1 = TenantFilters.WithSpecifications()
            .WithPagination(PaginationFilters.From(pageNumber: 1, pageSize: 5))
            .Build();

        /* act: get first page */
        var page1Results = await _tenantCollection.GetTenantsAsync(filtersPage1, CancellationToken.None);

        /* assert: page 1 should return exactly 5 tenants */
        Assert.Equal(5, page1Results.Count);

        /* arrange: prepare filters for page 2 with page size 5 */
        var filtersPage2 = TenantFilters.WithSpecifications()
            .WithPagination(PaginationFilters.From(pageNumber: 2, pageSize: 5))
            .Build();

        /* act: get second page */
        var page2Results = await _tenantCollection.GetTenantsAsync(filtersPage2, CancellationToken.None);

        /* assert: page 2 should return exactly 5 tenants */
        Assert.Equal(5, page2Results.Count);
    }

    [Fact(DisplayName = "[infrastructure] - when filtering tenants by client id, then only tenants with that client id are returned")]
    public async Task WhenFilteringTenantsByClientId_ThenOnlyTenantsFromThatClientAreReturned()
    {
        /* arrange: create tenant with a specific client id */
        var clientId = "test-client-id";

        var tenant = _fixture.Build<Tenant>()
            .With(tenant => tenant.Name, "Vinder")
            .With(tenant => tenant.ClientId, clientId)
            .With(tenant => tenant.IsDeleted, false)
            .Create();

        await _tenantCollection.InsertAsync(tenant);

        var filters = TenantFilters.WithSpecifications()
            .WithClientId(clientId)
            .WithName(tenant.Name)
            .Build();

        /* act: query tenants filtered by client id and name */
        var result = await _tenantCollection.GetTenantsAsync(filters, CancellationToken.None);
        var retrievedTenant = result.FirstOrDefault();

        /* assert: only tenant with matching client id is returned */
        Assert.NotNull(retrievedTenant);

        Assert.Equal(tenant.Id, retrievedTenant.Id);
        Assert.Equal(tenant.Name, retrievedTenant.Name);
        Assert.Equal(clientId, retrievedTenant.ClientId);
    }

    [Fact(DisplayName = "[infrastructure] - when counting tenants with filters, then count must reflect filtered records")]
    public async Task WhenCountingTenantsWithFilters_ThenCountMustReflectFilteredRecords()
    {
        /* arrange: create 10 tenants with varied clientId and IsDeleted */
        var clientId1 = "client1";
        var clientId2 = "client2";

        var tenants = new List<Tenant>();

        for (int index = 0; index < 10; index++)
        {
            var tenant = _fixture.Build<Tenant>()
                .With(tenant => tenant.ClientId, index < 5 ? clientId1 : clientId2) // first 5 client1, last 5 client2
                .With(tenant => tenant.Name, $"tenant{index}")
                .With(tenant => tenant.IsDeleted, index % 3 == 0) // every third tenant is deleted
                .Create();

            tenants.Add(tenant);

            await _tenantCollection.InsertAsync(tenant);
        }

        /* act: count tenants filtered by clientId1 and IsDeleted = false */
        var filters = TenantFilters.WithSpecifications()
            .WithClientId(clientId1)
            .WithIsDeleted(false)
            .Build();

        var filteredCount = await _tenantCollection.CountAsync(filters, CancellationToken.None);
        var expectedCount = tenants.Count(tenant => tenant.ClientId == clientId1 && !tenant.IsDeleted);

        /* assert: expected count of tenants for client */
        Assert.Equal(expectedCount, filteredCount);
    }

    #pragma warning disable S2325

    public async Task DisposeAsync() => await Task.CompletedTask;
    public async Task InitializeAsync()
    {
        await _mongoFixture.CleanDatabaseAsync();
    }
}
