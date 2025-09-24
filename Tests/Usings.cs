/* global using for System namespaces here */

global using System.Net;
global using System.Net.Http.Headers;
global using System.Net.Http.Json;

/* global using for Microsoft namespaces here */

global using Microsoft.AspNetCore.Mvc.Testing;
global using Microsoft.Extensions.DependencyInjection;

/* global using for Vinder namespaces here */

global using Vinder.IdentityProvider.Domain.Entities;
global using Vinder.IdentityProvider.Domain.Filters.Builders;
global using Vinder.IdentityProvider.Domain.Repositories;

global using Vinder.IdentityProvider.Application.Services;
global using Vinder.IdentityProvider.Application.Providers;

global using Vinder.IdentityProvider.Application.Payloads.Common;
global using Vinder.IdentityProvider.Application.Payloads.Identity;
global using Vinder.IdentityProvider.Application.Payloads.Group;
global using Vinder.IdentityProvider.Application.Payloads.Permission;
global using Vinder.IdentityProvider.Application.Payloads.Tenant;
global using Vinder.IdentityProvider.Application.Payloads.User;
global using Vinder.IdentityProvider.Application.Payloads.Scope;

global using Vinder.IdentityProvider.Infrastructure.Repositories;
global using Vinder.IdentityProvider.Infrastructure.Security;

global using Vinder.IdentityProvider.Common.Configuration;
global using Vinder.IdentityProvider.Common.Errors;
global using Vinder.IdentityProvider.Common.Results;
global using Vinder.IdentityProvider.Common.Utilities;
global using Vinder.IdentityProvider.Common.Constants;
global using Vinder.IdentityProvider.WebApi;

global using Vinder.IdentityProvider.TestSuite.Extensions;
global using Vinder.IdentityProvider.TestSuite.IntegrationTests.Fixtures;

/* global usings for third-party namespaces here */

global using DotNet.Testcontainers.Builders;
global using DotNet.Testcontainers.Containers;
global using MongoDB.Driver;
global using AutoFixture;
global using Moq;
