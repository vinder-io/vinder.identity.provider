global using System.Net;
global using System.Net.Http.Headers;
global using System.Net.Http.Json;

global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Cryptography;
global using System.Security.Claims;

global using Microsoft.AspNetCore.Mvc.Testing;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.Extensions.DependencyInjection;

global using Vinder.Internal.Essentials.Filtering;
global using Vinder.Internal.Essentials.Patterns;
global using Vinder.Internal.Essentials.Utilities;

global using Vinder.Federation.Domain.Aggregates;
global using Vinder.Federation.Domain.Filtering;
global using Vinder.Federation.Domain.Filtering.Builders;
global using Vinder.Federation.Domain.Collections;

global using Vinder.Federation.Application.Services;
global using Vinder.Federation.Application.Providers;

global using Vinder.Federation.Application.Payloads.Identity;
global using Vinder.Federation.Application.Payloads.User;
global using Vinder.Federation.Application.Payloads.Scope;
global using Vinder.Federation.Application.Payloads.Tenant;
global using Vinder.Federation.Application.Payloads.Permission;
global using Vinder.Federation.Application.Payloads.Group;
global using Vinder.Federation.Application.Payloads.Common;
global using Vinder.Federation.Application.Payloads.OpenID;

global using Vinder.Federation.Infrastructure.Persistence;
global using Vinder.Federation.Infrastructure.Security;
global using Vinder.Federation.Infrastructure.Constants;

global using Vinder.Federation.Domain.Errors;
global using Vinder.Federation.WebApi;

global using Vinder.Federation.TestSuite.Extensions;
global using Vinder.Federation.TestSuite.Integration.Fixtures;

global using Xunit;
global using DotNet.Testcontainers.Builders;
global using DotNet.Testcontainers.Containers;
global using MongoDB.Driver;
global using AutoFixture;
global using Moq;
