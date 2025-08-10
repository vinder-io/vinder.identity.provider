/* global using for Vinder namespaces here */

global using Vinder.IdentityProvider.Domain.Entities;
global using Vinder.IdentityProvider.Domain.Filters.Builders;
global using Vinder.IdentityProvider.Domain.Repositories;

global using Vinder.IdentityProvider.Application.Services;
global using Vinder.IdentityProvider.Application.Payloads.Identity;
global using Vinder.IdentityProvider.Application.Providers;

global using Vinder.IdentityProvider.Infrastructure.Repositories;
global using Vinder.IdentityProvider.Infrastructure.Security;
global using Vinder.IdentityProvider.Infrastructure.IoC.Helpers;

global using Vinder.IdentityProvider.Common.Configuration;
global using Vinder.IdentityProvider.Common.Errors;
global using Vinder.IdentityProvider.TestSuite.IntegrationTests.Fixtures;

/* global usings for third-party namespaces here */

global using DotNet.Testcontainers.Builders;
global using DotNet.Testcontainers.Containers;
global using MongoDB.Driver;
global using AutoFixture;
global using Moq;
