/* global using for System namespaces here */

global using System.Net;
global using System.Net.Http.Headers;
global using System.Net.Http.Json;

/* global using for Microsoft namespaces here */

global using Microsoft.AspNetCore.Mvc.Testing;
global using Microsoft.Extensions.DependencyInjection;

/* global using for Vinder namespaces here */

global using Vinder.Internal.Essentials.Filters;
global using Vinder.Internal.Essentials.Patterns;
global using Vinder.Internal.Essentials.Utilities;

global using Vinder.Identity.Domain.Aggregates;
global using Vinder.Identity.Domain.Filtering;
global using Vinder.Identity.Domain.Filtering.Builders;
global using Vinder.Identity.Domain.Repositories;

global using Vinder.Identity.Application.Services;
global using Vinder.Identity.Application.Providers;

global using Vinder.Identity.Application.Payloads.Identity;
global using Vinder.Identity.Application.Payloads.User;
global using Vinder.Identity.Application.Payloads.Scope;

global using Vinder.Identity.Infrastructure.Repositories;
global using Vinder.Identity.Infrastructure.Security;

global using Vinder.Identity.Domain.Errors;
global using Vinder.Identity.WebApi;

global using Vinder.Identity.TestSuite.Extensions;
global using Vinder.Identity.TestSuite.IntegrationTests.Fixtures;

/* global usings for third-party namespaces here */

global using DotNet.Testcontainers.Builders;
global using DotNet.Testcontainers.Containers;
global using MongoDB.Driver;
global using AutoFixture;
global using Moq;
