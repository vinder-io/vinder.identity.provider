/* global usings for System namespaces here */

global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Security.Cryptography;

/* global usings for Microsoft namespaces here */

global using Microsoft.IdentityModel.Tokens;

/* global usings for Vinder namespaces here */

global using Vinder.Internal.Infrastructure.Persistence;
global using Vinder.Internal.Infrastructure.Persistence.Pipelines;
global using Vinder.Internal.Infrastructure.Persistence.Repositories;

global using Vinder.IdentityProvider.Domain.Entities;
global using Vinder.IdentityProvider.Domain.Repositories;
global using Vinder.IdentityProvider.Domain.Filtering;
global using Vinder.IdentityProvider.Domain.Filtering.Builders;

global using Vinder.IdentityProvider.Infrastructure.Constants;
global using Vinder.IdentityProvider.Infrastructure.Pipelines;
global using Vinder.IdentityProvider.Infrastructure.Utilities;

global using Vinder.IdentityProvider.Application.Payloads.Identity;
global using Vinder.IdentityProvider.Application.Services;
global using Vinder.IdentityProvider.Application.Providers;
global using Vinder.IdentityProvider.Application.Payloads.Client;

global using Vinder.IdentityProvider.Common.Errors;
global using Vinder.IdentityProvider.Common.Results;

global using SecurityToken = Vinder.IdentityProvider.Domain.Entities.SecurityToken;

/* global usings for third-party namespaces here */

global using MongoDB.Driver;
global using MongoDB.Bson;
global using MongoDB.Bson.Serialization;