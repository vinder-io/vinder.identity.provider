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
global using Vinder.Internal.Essentials.Patterns;

global using Vinder.Identity.Domain.Entities;
global using Vinder.Identity.Domain.Errors;
global using Vinder.Identity.Domain.Repositories;
global using Vinder.Identity.Domain.Filtering;
global using Vinder.Identity.Domain.Filtering.Builders;

global using Vinder.Identity.Infrastructure.Constants;
global using Vinder.Identity.Infrastructure.Pipelines;

global using Vinder.Identity.Application.Payloads.Identity;
global using Vinder.Identity.Application.Services;
global using Vinder.Identity.Application.Providers;
global using Vinder.Identity.Application.Payloads.Client;

global using SecurityToken = Vinder.Identity.Domain.Entities.SecurityToken;

/* global usings for third-party namespaces here */

global using MongoDB.Driver;
global using MongoDB.Bson;
global using MongoDB.Bson.Serialization;