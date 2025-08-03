namespace Vinder.IdentityProvider.TestSuite.IntegrationTests.Repositories;

public sealed class TokenRepositoryTests : IClassFixture<MongoDatabaseFixture>, IAsyncLifetime
{
    private readonly ITokenRepository _tokenRepository;
    private readonly IMongoDatabase _database;
    private readonly MongoDatabaseFixture _mongoFixture;
    private readonly Fixture _fixture = new();

    public TokenRepositoryTests(MongoDatabaseFixture fixture)
    {
        _mongoFixture = fixture;
        _database = fixture.Database;
        _tokenRepository = new TokenRepository(_database);
    }

    [Fact(DisplayName = "[infrastructure] - when inserting a token, then it must persist in the database")]
    public async Task WhenInsertingAToken_ThenItMustPersistInTheDatabase()
    {
        /* arrange: create token and matching filter */
        var token = _fixture.Build<SecurityToken>()
            .With(token => token.Value, "some-secret-token")
            .With(token => token.IsDeleted, false)
            .Create();

        var filters = new TokenFiltersBuilder()
            .WithValue(token.Value)
            .Build();

        /* act: persist token and query using value filter */
        await _tokenRepository.InsertAsync(token);

        var result = await _tokenRepository.GetTokensAsync(filters, CancellationToken.None);
        var retrievedToken = result.FirstOrDefault();

        /* assert: token must be retrieved with same id and value */
        Assert.NotNull(retrievedToken);
        Assert.Equal(token.Id, retrievedToken.Id);
        Assert.Equal(token.Value, retrievedToken.Value);
    }

    [Fact(DisplayName = "[infrastructure] - when updating a token, then updated fields must persist")]
    public async Task WhenUpdatingAToken_ThenUpdatedFieldsMustPersist()
    {
        /* arrange: create and insert token */
        var token = _fixture.Build<SecurityToken>()
            .With(token => token.Value, "update.test-token")
            .With(token => token.IsDeleted, false)
            .Create();

        await _tokenRepository.InsertAsync(token);

        /* act: update value and save */
        var newValue = "updated-token-value";

        token.Value = newValue;

        await _tokenRepository.UpdateAsync(token);

        var filters = new TokenFiltersBuilder()
            .WithValue(newValue)
            .Build();

        var result = await _tokenRepository.GetTokensAsync(filters, CancellationToken.None);
        var updatedToken = result.FirstOrDefault();

        /* assert: updated token must be found with new value */
        Assert.NotNull(updatedToken);

        Assert.Equal(token.Id, updatedToken.Id);
        Assert.Equal(newValue, updatedToken.Value);
    }

    [Fact(DisplayName = "[infrastructure] - when deleting a token, then it must be marked as deleted and not returned by filters")]
    public async Task WhenDeletingAToken_ThenItMustBeMarkedDeletedAndExcludedFromResults()
    {
        /* arrange: create and insert token */
        var token = _fixture.Build<SecurityToken>()
            .With(token => token.Value, "delete.test-token")
            .With(token => token.IsDeleted, false)
            .Create();

        await _tokenRepository.InsertAsync(token);

        var filters = new TokenFiltersBuilder()
            .WithValue(token.Value)
            .Build();

        /* act: delete token and query by value */
        var deleted = await _tokenRepository.DeleteAsync(token);

        var resultAfterDelete = await _tokenRepository.GetTokensAsync(filters, CancellationToken.None);

        /* assert: no tokens should be returned after delete */
        Assert.DoesNotContain(resultAfterDelete, token => token.Id == token.Id);

        /* arrange: prepare filters including deleted tokens */
        var filtersWithDeleted = new TokenFiltersBuilder()
            .WithValue(token.Value)
            .WithIsDeleted(true)
            .Build();

        /* act: refetch tokens including deleted */
        var resultWithDeleted = await _tokenRepository.GetTokensAsync(filtersWithDeleted, CancellationToken.None);

        /* assert: token should be returned when including deleted tokens */
        Assert.Contains(resultWithDeleted, token => token.Id == token.Id);

        Assert.True(token.IsDeleted);
        Assert.True(deleted);
    }

    [Fact(DisplayName = "[infrastructure] - when filtering tokens, then it must return matching tokens")]
    public async Task WhenFilteringTokens_ThenItMustReturnOnlyMatchingTokens()
    {
        /* arrange: insert two tokens with different values */
        var token1 = _fixture.Build<SecurityToken>()
            .With(token => token.Value, "filter1-token")
            .With(token => token.IsDeleted, false)
            .Create();

        var token2 = _fixture.Build<SecurityToken>()
            .With(token => token.Value, "filter2-token")
            .With(token => token.IsDeleted, false)
            .Create();

        await _tokenRepository.InsertAsync(token1);
        await _tokenRepository.InsertAsync(token2);

        var filters = new TokenFiltersBuilder()
            .WithValue("filter1-token")
            .Build();

        /* act: query tokens filtered by value */
        var filteredTokens = await _tokenRepository.GetTokensAsync(filters, CancellationToken.None);

        /* assert: only token1 should be returned */
        Assert.Single(filteredTokens);
        Assert.Equal(token1.Id, filteredTokens.First().Id);
    }

    [Fact(DisplayName = "[infrastructure] - when paginating 10 tokens with page size 5, then it must return 5 tokens per page")]
    public async Task WhenPaginatingTenTokens_ThenItMustReturnFiveTokensPerPage()
    {
        /* arrange: create and insert 10 tokens, all not deleted */
        var tokens = Enumerable.Range(1, 10)
            .Select(index => _fixture.Build<SecurityToken>()
            .With(token => token.Value, $"token-{index}")
            .With(token => token.IsDeleted, false)
            .Create())
            .ToList();

        foreach (var token in tokens)
        {
            await _tokenRepository.InsertAsync(token);
        }

        /* arrange: prepare filters for page 1 with page size 5 */
        var filtersPage1 = new TokenFiltersBuilder()
            .WithPageSize(5)
            .WithPageNumber(1)
            .Build();

        /* act: get first page */
        var page1Results = await _tokenRepository.GetTokensAsync(filtersPage1, CancellationToken.None);

        /* assert: page 1 should return exactly 5 tokens */
        Assert.Equal(5, page1Results.Count);

        /* arrange: prepare filters for page 2 with page size 5 */
        var filtersPage2 = new TokenFiltersBuilder()
            .WithPageSize(5)
            .WithPageNumber(2)
            .Build();

        /* act: get second page */
        var page2Results = await _tokenRepository.GetTokensAsync(filtersPage2, CancellationToken.None);

        /* assert: page 2 should return exactly 5 tokens */
        Assert.Equal(5, page2Results.Count);
    }

    [Fact(DisplayName = "[infrastructure] - when filtering tokens by tenant id, then only tokens from that tenant are returned")]
    public async Task WhenFilteringTokensByTenantId_ThenOnlyTokensFromThatTenantAreReturned()
    {
        /* arrange: create tenant and token with tenant id */
        var tenantId = Guid.NewGuid();

        var token = _fixture.Build<SecurityToken>()
            .With(token => token.Value, "some-tenant-token")
            .With(token => token.TenantId, tenantId)
            .With(token => token.IsDeleted, false)
            .Create();

        await _tokenRepository.InsertAsync(token);

        var filters = new TokenFiltersBuilder()
            .WithTenantId(tenantId)
            .WithValue(token.Value)
            .Build();

        /* act: query tokens filtered by tenant id and value */
        var result = await _tokenRepository.GetTokensAsync(filters, CancellationToken.None);
        var retrievedToken = result.FirstOrDefault();

        /* assert: only token with matching tenant id is returned */
        Assert.NotNull(retrievedToken);

        Assert.Equal(token.Id, retrievedToken.Id);
        Assert.Equal(token.Value, retrievedToken.Value);
        Assert.Equal(tenantId, retrievedToken.TenantId);
    }

    [Theory(DisplayName = "[infrastructure] - when filtering tokens by type, then it must return matching tokens")]
    [InlineData(TokenType.Refresh)]
    [InlineData(TokenType.EmailVerification)]
    [InlineData(TokenType.PasswordReset)]
    public async Task WhenFilteringTokensByType_ThenItMustReturnOnlyMatchingTokens(TokenType type)
    {
        // arrange: insert one token with the requested type, and one with a different type
        var matchingToken = _fixture.Build<SecurityToken>()
            .With(token => token.Type, type)
            .With(token => token.IsDeleted, false)
            .Create();

        var nonMatchingToken = _fixture.Build<SecurityToken>()
            .With(token => token.Type, type switch
            {
                TokenType.Refresh => TokenType.EmailVerification,
                TokenType.EmailVerification => TokenType.PasswordReset,
                TokenType.PasswordReset => TokenType.Refresh,

                _ => TokenType.Refresh
            })
            .With(token => token.IsDeleted, false)
            .Create();

        await _tokenRepository.InsertAsync(matchingToken);
        await _tokenRepository.InsertAsync(nonMatchingToken);

        var filters = new TokenFiltersBuilder()
            .WithType(type)
            .Build();

        // act: query filtered by token type
        var result = await _tokenRepository.GetTokensAsync(filters, CancellationToken.None);

        // assert: only the matching token should be returned
        Assert.Single(result);

        Assert.Equal(matchingToken.Type, result.First().Type);
        Assert.Equal(matchingToken.Id, result.First().Id);
    }


    [Fact(DisplayName = "[infrastructure] - when filtering tokens by user id, then only tokens from that user are returned")]
    public async Task WhenFilteringTokensByUserId_ThenOnlyTokensFromThatUserAreReturned()
    {
        /* arrange: create user and token with user id */
        var userId = Guid.NewGuid();

        var token = _fixture.Build<SecurityToken>()
            .With(token => token.Value, "some-user-token")
            .With(token => token.UserId, userId)
            .With(token => token.IsDeleted, false)
            .Create();

        await _tokenRepository.InsertAsync(token);

        var filters = new TokenFiltersBuilder()
            .WithUserId(userId)
            .WithValue(token.Value)
            .Build();

        /* act: query tokens filtered by user id and value */
        var result = await _tokenRepository.GetTokensAsync(filters, CancellationToken.None);
        var retrievedToken = result.FirstOrDefault();

        /* assert: only token with matching user id is returned */
        Assert.NotNull(retrievedToken);

        Assert.Equal(token.Id, retrievedToken.Id);
        Assert.Equal(token.Value, retrievedToken.Value);
        Assert.Equal(userId, retrievedToken.UserId);
    }

    [Fact(DisplayName = "[infrastructure] - when counting tokens with filters, then count must reflect filtered records")]
    public async Task WhenCountingTokensWithFilters_ThenCountMustReflectFilteredRecords()
    {
        /* arrange: create 10 tokens with varied tenantId, userId and IsDeleted */
        var tenantId1 = Guid.NewGuid();
        var tenantId2 = Guid.NewGuid();

        var userId1 = Guid.NewGuid();
        var userId2 = Guid.NewGuid();

        var tokens = new List<SecurityToken>();

        for (int index = 0; index < 10; index++)
        {
            var token = _fixture.Build<SecurityToken>()
                .With(token => token.TenantId, index < 5 ? tenantId1 : tenantId2) // first 5 tenant1, last 5 tenant2
                .With(token => token.UserId, index % 2 == 0 ? userId1 : userId2) // alternate user
                .With(token => token.Value, $"token{index}")
                .With(token => token.IsDeleted, index % 3 == 0) // every third token is deleted
                .Create();

            tokens.Add(token);

            await _tokenRepository.InsertAsync(token);
        }

        /* act: count tokens filtered by tenant1 and IsDeleted = false */
        var filters = new TokenFiltersBuilder()
            .WithTenantId(tenantId1)
            .WithIsDeleted(false)
            .Build();

        var filteredCount = await _tokenRepository.CountAsync(filters);
        var expectedCount = tokens.Count(token => token.TenantId == tenantId1 && !token.IsDeleted);

        /* assert: expected count of tokens for tenant*/
        Assert.Equal(expectedCount, filteredCount);
    }

    public async Task DisposeAsync() => await Task.CompletedTask;
    public async Task InitializeAsync()
    {
        await _mongoFixture.CleanDatabaseAsync();
    }
}
