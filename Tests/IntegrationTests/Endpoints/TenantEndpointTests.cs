namespace Vinder.Identity.TestSuite.IntegrationTests.Endpoints;

public sealed class TenantEndpointTests(IntegrationEnvironmentFixture factory) :
    IClassFixture<IntegrationEnvironmentFixture>
{
    private readonly Fixture _fixture = new();

    [Fact(DisplayName = "[e2e] - when POST /tenants/{id}/permissions with valid permission should assign permission successfully")]
    public async Task WhenPostTenantPermissionsWithValidPermission_ShouldAssignPermissionSuccessfully()
    {
        /* arrange: authenticate user and get access token */
        var httpClient = factory.HttpClient.WithTenantHeader("master");
        var credentials = new AuthenticationCredentials
        {
            Username = "vinder.testing.user",
            Password = "vinder.testing.password"
        };

        var authenticationResponse = await httpClient.PostAsJsonAsync("api/v1/identity/authenticate", credentials);
        var authenticationResult = await authenticationResponse.Content.ReadFromJsonAsync<AuthenticationResult>();

        Assert.NotNull(authenticationResult);
        Assert.NotEmpty(authenticationResult.AccessToken);

        httpClient.WithAuthorization(authenticationResult.AccessToken);

        /* arrange: create a new tenant */
        var tenantPayload = _fixture.Build<TenantCreationScheme>()
            .With(tenant => tenant.Name, $"test-tenant-{Guid.NewGuid()}")
            .Create();

        var tenantResponse = await httpClient.PostAsJsonAsync("api/v1/tenants", tenantPayload);
        var tenant = await tenantResponse.Content.ReadFromJsonAsync<TenantDetailsScheme>();

        Assert.NotNull(tenant);
        Assert.Equal(HttpStatusCode.Created, tenantResponse.StatusCode);

        /* arrange: create a new permission */
        var permissionPayload = _fixture.Build<PermissionCreationScheme>()
            .With(permission => permission.Name, $"test.permission.{Guid.NewGuid()}")
            .Create();

        var permissionResponse = await httpClient.PostAsJsonAsync("api/v1/permissions", permissionPayload);
        var permission = await permissionResponse.Content.ReadFromJsonAsync<PermissionDetailsScheme>();

        Assert.NotNull(permission);
        Assert.Equal(HttpStatusCode.Created, permissionResponse.StatusCode);

        /* arrange: prepare request to assign permission to tenant */
        var assignPermissionPayload = new AssignTenantPermissionScheme
        {
            PermissionName = permission.Name
        };

        /* act: send POST request to assign permission to tenant */
        var assignResponse = await httpClient.PostAsJsonAsync($"api/v1/tenants/{tenant.Id}/permissions", assignPermissionPayload);
        var assignedPermissions = await assignResponse.Content.ReadFromJsonAsync<IReadOnlyCollection<PermissionDetailsScheme>>();

        /* assert: response should be 200 OK and permissions list should be returned */
        Assert.Equal(HttpStatusCode.OK, assignResponse.StatusCode);
        Assert.NotNull(assignedPermissions);

        /* assert: the assigned permission should be in the returned list */
        Assert.Contains(assignedPermissions, p => p.Name == permission.Name);
    }

    [Fact(DisplayName = "[e2e] - when POST /tenants/{id}/permissions with non-existent tenant should return 404 #VINDER-IDP-ERR-TNT-404")]
    public async Task WhenPostTenantPermissionsWithNonExistentTenant_ShouldReturnNotFound()
    {
        /* arrange: authenticate user and get access token */
        var httpClient = factory.HttpClient.WithTenantHeader("master");
        var credentials = new AuthenticationCredentials
        {
            Username = "vinder.testing.user",
            Password = "vinder.testing.password"
        };

        var authenticationResponse = await httpClient.PostAsJsonAsync("api/v1/identity/authenticate", credentials);
        var authenticationResult = await authenticationResponse.Content.ReadFromJsonAsync<AuthenticationResult>();

        Assert.NotNull(authenticationResult);
        Assert.NotEmpty(authenticationResult.AccessToken);

        httpClient.WithAuthorization(authenticationResult.AccessToken);

        /* arrange: prepare request with a non-existent tenant ID */
        var nonExistentTenantId = Guid.NewGuid().ToString();
        var assignPermissionPayload = new AssignTenantPermissionScheme
        {
            PermissionName = "some.permission"
        };

        /* act: send POST request to assign permission to non-existent tenant */
        var assignResponse = await httpClient.PostAsJsonAsync($"api/v1/tenants/{nonExistentTenantId}/permissions", assignPermissionPayload);
        var error = await assignResponse.Content.ReadFromJsonAsync<Error>();

        /* assert: response should be 404 Not Found */
        Assert.NotNull(error);

        Assert.Equal(HttpStatusCode.NotFound, assignResponse.StatusCode);
        Assert.Equal(TenantErrors.TenantDoesNotExist, error);
    }

    [Fact(DisplayName = "[e2e] - when POST /tenants/{id}/permissions with non-existent permission should return 404 #VINDER-IDP-ERR-PRM-404")]
    public async Task WhenPostTenantPermissionsWithNonExistentPermission_ShouldReturnNotFound()
    {
        /* arrange: authenticate user and get access token */
        var httpClient = factory.HttpClient.WithTenantHeader("master");
        var credentials = new AuthenticationCredentials
        {
            Username = "vinder.testing.user",
            Password = "vinder.testing.password"
        };

        var authenticationResponse = await httpClient.PostAsJsonAsync("api/v1/identity/authenticate", credentials);
        var authenticationResult = await authenticationResponse.Content.ReadFromJsonAsync<AuthenticationResult>();

        Assert.NotNull(authenticationResult);
        Assert.NotEmpty(authenticationResult.AccessToken);

        httpClient.WithAuthorization(authenticationResult.AccessToken);

        /* arrange: create a new tenant */
        var tenantPayload = _fixture.Build<TenantCreationScheme>()
            .With(tenant => tenant.Name, $"test-tenant-{Guid.NewGuid()}")
            .Create();

        var tenantResponse = await httpClient.PostAsJsonAsync("api/v1/tenants", tenantPayload);
        var tenant = await tenantResponse.Content.ReadFromJsonAsync<TenantDetailsScheme>();

        Assert.NotNull(tenant);
        Assert.Equal(HttpStatusCode.Created, tenantResponse.StatusCode);

        /* arrange: prepare request with a non-existent permission name */
        var assignPermissionPayload = new AssignTenantPermissionScheme
        {
            PermissionName = $"non.existent.permission.{Guid.NewGuid()}"
        };

        /* act: send POST request to assign non-existent permission to tenant */
        var assignResponse = await httpClient.PostAsJsonAsync($"api/v1/tenants/{tenant.Id}/permissions", assignPermissionPayload);
        var error = await assignResponse.Content.ReadFromJsonAsync<Error>();

        /* assert: response should be 404 Not Found */
        Assert.NotNull(error);

        Assert.Equal(HttpStatusCode.NotFound, assignResponse.StatusCode);
        Assert.Equal(PermissionErrors.PermissionDoesNotExist, error);
    }

    [Fact(DisplayName = "[e2e] - when POST /tenants/{id}/permissions with duplicate permission should return 409 #VINDER-IDP-ERR-TNT-415")]
    public async Task WhenPostTenantPermissionsWithDuplicatePermission_ShouldReturnConflict()
    {
        /* arrange: authenticate user and get access token */
        var httpClient = factory.HttpClient.WithTenantHeader("master");
        var credentials = new AuthenticationCredentials
        {
            Username = "vinder.testing.user",
            Password = "vinder.testing.password"
        };

        var authenticationResponse = await httpClient.PostAsJsonAsync("api/v1/identity/authenticate", credentials);
        var authenticationResult = await authenticationResponse.Content.ReadFromJsonAsync<AuthenticationResult>();

        Assert.NotNull(authenticationResult);
        Assert.NotEmpty(authenticationResult.AccessToken);

        httpClient.WithAuthorization(authenticationResult.AccessToken);

        /* arrange: create a new tenant */
        var tenantPayload = _fixture.Build<TenantCreationScheme>()
            .With(tenant => tenant.Name, $"test-tenant-{Guid.NewGuid()}")
            .Create();

        var tenantResponse = await httpClient.PostAsJsonAsync("api/v1/tenants", tenantPayload);
        var tenant = await tenantResponse.Content.ReadFromJsonAsync<TenantDetailsScheme>();

        Assert.NotNull(tenant);
        Assert.Equal(HttpStatusCode.Created, tenantResponse.StatusCode);

        /* arrange: create a new permission */
        var permissionPayload = _fixture.Build<PermissionCreationScheme>()
            .With(permission => permission.Name, $"test.permission.{Guid.NewGuid()}")
            .Create();

        var permissionResponse = await httpClient.PostAsJsonAsync("api/v1/permissions", permissionPayload);
        var permission = await permissionResponse.Content.ReadFromJsonAsync<PermissionDetailsScheme>();

        Assert.NotNull(permission);
        Assert.Equal(HttpStatusCode.Created, permissionResponse.StatusCode);

        /* arrange: assign permission to tenant first time */
        var assignPermissionPayload = new AssignTenantPermissionScheme
        {
            PermissionName = permission.Name
        };

        var firstAssignResponse = await httpClient.PostAsJsonAsync($"api/v1/tenants/{tenant.Id}/permissions", assignPermissionPayload);

        Assert.Equal(HttpStatusCode.OK, firstAssignResponse.StatusCode);

        /* act: attempt to assign the same permission again */
        var secondAssignResponse = await httpClient.PostAsJsonAsync($"api/v1/tenants/{tenant.Id}/permissions", assignPermissionPayload);
        var error = await secondAssignResponse.Content.ReadFromJsonAsync<Error>();

        /* assert: response should be 409 Conflict */
        Assert.NotNull(error);

        Assert.Equal(HttpStatusCode.Conflict, secondAssignResponse.StatusCode);
        Assert.Equal(TenantErrors.TenantAlreadyHasPermission, error);
    }

    [Fact(DisplayName = "[e2e] - when GET /tenants/{id}/permissions should return tenant's assigned permissions")]
    public async Task WhenGetTenantPermissions_ShouldReturnAssignedPermissions()
    {
        /* arrange: authenticate user and get access token */
        var httpClient = factory.HttpClient.WithTenantHeader("master");
        var credentials = new AuthenticationCredentials
        {
            Username = "vinder.testing.user",
            Password = "vinder.testing.password"
        };

        var authenticationResponse = await httpClient.PostAsJsonAsync("api/v1/identity/authenticate", credentials);
        var authenticationResult = await authenticationResponse.Content.ReadFromJsonAsync<AuthenticationResult>();

        Assert.NotNull(authenticationResult);
        Assert.NotEmpty(authenticationResult.AccessToken);

        httpClient.WithAuthorization(authenticationResult.AccessToken);

        /* arrange: create a new tenant */
        var tenantPayload = _fixture.Build<TenantCreationScheme>()
            .With(tenant => tenant.Name, $"test-tenant-{Guid.NewGuid()}")
            .Create();

        var tenantResponse = await httpClient.PostAsJsonAsync("api/v1/tenants", tenantPayload);
        var tenant = await tenantResponse.Content.ReadFromJsonAsync<TenantDetailsScheme>();

        Assert.NotNull(tenant);
        Assert.Equal(HttpStatusCode.Created, tenantResponse.StatusCode);

        /* arrange: create and assign multiple permissions */
        var permissionNames = new List<string>();

        for (int index = 0; index < 3; index++)
        {
            var permissionPayload = _fixture.Build<PermissionCreationScheme>()
                .With(permission => permission.Name, $"test.permission.{Guid.NewGuid()}")
                .Create();

            var permissionResponse = await httpClient.PostAsJsonAsync("api/v1/permissions", permissionPayload);
            var permission = await permissionResponse.Content.ReadFromJsonAsync<PermissionDetailsScheme>();

            Assert.NotNull(permission);

            permissionNames.Add(permission.Name);

            var assignPayload = new AssignTenantPermissionScheme { PermissionName = permission.Name };

            await httpClient.PostAsJsonAsync($"api/v1/tenants/{tenant.Id}/permissions", assignPayload);
        }

        /* act: send GET request to retrieve tenant's permissions */
        var getResponse = await httpClient.GetAsync($"api/v1/tenants/{tenant.Id}/permissions");
        var permissions = await getResponse.Content.ReadFromJsonAsync<IReadOnlyCollection<PermissionDetailsScheme>>();

        /* assert: response should be 200 OK */
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        Assert.NotNull(permissions);

        foreach (var permissionName in permissionNames)
        {
            Assert.Contains(permissions, permission => permission.Name == permissionName);
        }
    }

    [Fact(DisplayName = "[e2e] - when GET /tenants/{id}/permissions with non-existent tenant should return 404 #VINDER-IDP-ERR-TNT-404")]
    public async Task WhenGetTenantPermissionsWithNonExistentTenant_ShouldReturnNotFound()
    {
        /* arrange: authenticate user and get access token */
        var httpClient = factory.HttpClient.WithTenantHeader("master");
        var credentials = new AuthenticationCredentials
        {
            Username = "vinder.testing.user",
            Password = "vinder.testing.password"
        };

        var authenticationResponse = await httpClient.PostAsJsonAsync("api/v1/identity/authenticate", credentials);
        var authenticationResult = await authenticationResponse.Content.ReadFromJsonAsync<AuthenticationResult>();

        Assert.NotNull(authenticationResult);
        Assert.NotEmpty(authenticationResult.AccessToken);

        httpClient.WithAuthorization(authenticationResult.AccessToken);

        /* arrange: prepare request with a non-existent tenant ID */
        var nonExistentTenantId = Guid.NewGuid().ToString();

        /* act: send GET request for non-existent tenant's permissions */
        var response = await httpClient.GetAsync($"api/v1/tenants/{nonExistentTenantId}/permissions");
        var error = await response.Content.ReadFromJsonAsync<Error>();

        /* assert: response should be 404 Not Found */

        Assert.NotNull(error);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal(TenantErrors.TenantDoesNotExist.Code, error.Code);
    }

    [Fact(DisplayName = "[e2e] - when DELETE /tenants/{id}/permissions/{permissionId} should revoke permission successfully")]
    public async Task WhenDeleteTenantPermission_ShouldRevokePermissionSuccessfully()
    {
        /* arrange: authenticate user and get access token */
        var httpClient = factory.HttpClient.WithTenantHeader("master");
        var credentials = new AuthenticationCredentials
        {
            Username = "vinder.testing.user",
            Password = "vinder.testing.password"
        };

        var authenticationResponse = await httpClient.PostAsJsonAsync("api/v1/identity/authenticate", credentials);
        var authenticationResult = await authenticationResponse.Content.ReadFromJsonAsync<AuthenticationResult>();

        Assert.NotNull(authenticationResult);
        Assert.NotEmpty(authenticationResult.AccessToken);

        httpClient.WithAuthorization(authenticationResult.AccessToken);

        /* arrange: create a new tenant */
        var tenantPayload = _fixture.Build<TenantCreationScheme>()
            .With(tenant => tenant.Name, $"test-tenant-{Guid.NewGuid()}")
            .Create();

        var tenantResponse = await httpClient.PostAsJsonAsync("api/v1/tenants", tenantPayload);
        var tenant = await tenantResponse.Content.ReadFromJsonAsync<TenantDetailsScheme>();

        Assert.NotNull(tenant);
        Assert.Equal(HttpStatusCode.Created, tenantResponse.StatusCode);

        /* arrange: create a new permission */
        var permissionPayload = _fixture.Build<PermissionCreationScheme>()
            .With(permission => permission.Name, $"test.permission.{Guid.NewGuid()}")
            .Create();

        var permissionResponse = await httpClient.PostAsJsonAsync("api/v1/permissions", permissionPayload);
        var permission = await permissionResponse.Content.ReadFromJsonAsync<PermissionDetailsScheme>();

        Assert.NotNull(permission);
        Assert.Equal(HttpStatusCode.Created, permissionResponse.StatusCode);

        /* arrange: assign permission to tenant */
        var payload = new AssignTenantPermissionScheme { PermissionName = permission.Name };
        var response = await httpClient.PostAsJsonAsync($"api/v1/tenants/{tenant.Id}/permissions", payload);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        /* act: send DELETE request to revoke permission from tenant */
        var deleteResponse = await httpClient.DeleteAsync($"api/v1/tenants/{tenant.Id}/permissions/{permission.Id}");

        /* assert: response should be 204 No Content */
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        /* assert: verify permission is no longer in tenant's permissions list */
        var httpResponse = await httpClient.GetAsync($"api/v1/tenants/{tenant.Id}/permissions");
        var permissions = await httpResponse.Content.ReadFromJsonAsync<IReadOnlyCollection<PermissionDetailsScheme>>();

        Assert.NotNull(permissions);
        Assert.DoesNotContain(permissions, p => p.Id == permission.Id);
    }

    [Fact(DisplayName = "[e2e] - when DELETE /tenants/{id}/permissions/{permissionId} with non-existent tenant should return 404 #VINDER-IDP-ERR-TNT-404")]
    public async Task WhenDeleteTenantPermissionWithNonExistentTenant_ShouldReturnNotFound()
    {
        /* arrange: authenticate user and get access token */
        var httpClient = factory.HttpClient.WithTenantHeader("master");
        var credentials = new AuthenticationCredentials
        {
            Username = "vinder.testing.user",
            Password = "vinder.testing.password"
        };

        var authenticationResponse = await httpClient.PostAsJsonAsync("api/v1/identity/authenticate", credentials);
        var authenticationResult = await authenticationResponse.Content.ReadFromJsonAsync<AuthenticationResult>();

        Assert.NotNull(authenticationResult);
        Assert.NotEmpty(authenticationResult.AccessToken);

        httpClient.WithAuthorization(authenticationResult.AccessToken);

        /* arrange: prepare request with a non-existent tenant ID */
        var nonExistentTenantId = Guid.NewGuid().ToString();
        var nonExistentPermissionId = Guid.NewGuid().ToString();

        /* act: send DELETE request for non-existent tenant */
        var response = await httpClient.DeleteAsync($"api/v1/tenants/{nonExistentTenantId}/permissions/{nonExistentPermissionId}");
        var error = await response.Content.ReadFromJsonAsync<Error>();

        /* assert: response should be 404 Not Found */
        Assert.NotNull(error);

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        Assert.Equal(TenantErrors.TenantDoesNotExist, error);
    }

    [Fact(DisplayName = "[e2e] - when DELETE /tenants/{id}/permissions/{permissionId} with non-existent permission should return 404 #VINDER-IDP-ERR-PRM-404")]
    public async Task WhenDeleteTenantPermissionWithNonExistentPermission_ShouldReturnNotFound()
    {
        /* arrange: authenticate user and get access token */
        var httpClient = factory.HttpClient.WithTenantHeader("master");
        var credentials = new AuthenticationCredentials
        {
            Username = "vinder.testing.user",
            Password = "vinder.testing.password"
        };

        var authenticationResponse = await httpClient.PostAsJsonAsync("api/v1/identity/authenticate", credentials);
        var authenticationResult = await authenticationResponse.Content.ReadFromJsonAsync<AuthenticationResult>();

        Assert.NotNull(authenticationResult);
        Assert.NotEmpty(authenticationResult.AccessToken);

        httpClient.WithAuthorization(authenticationResult.AccessToken);

        /* arrange: create a new tenant */
        var tenantPayload = _fixture.Build<TenantCreationScheme>()
            .With(tenant => tenant.Name, $"test-tenant-{Guid.NewGuid()}")
            .Create();

        var response = await httpClient.PostAsJsonAsync("api/v1/tenants", tenantPayload);
        var tenant = await response.Content.ReadFromJsonAsync<TenantDetailsScheme>();

        Assert.NotNull(tenant);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        /* arrange: prepare request with a non-existent permission ID */
        var nonExistentPermissionId = Guid.NewGuid().ToString();

        /* act: send DELETE request with non-existent permission */
        var httpResponse = await httpClient.DeleteAsync($"api/v1/tenants/{tenant.Id}/permissions/{nonExistentPermissionId}");
        var error = await httpResponse.Content.ReadFromJsonAsync<Error>();

        /* assert: response should be 404 Not Found */
        Assert.NotNull(error);

        Assert.Equal(HttpStatusCode.NotFound, httpResponse.StatusCode);
        Assert.Equal(PermissionErrors.PermissionDoesNotExist, error);
    }
}
