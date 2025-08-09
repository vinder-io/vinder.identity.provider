/* global usings for System namespaces here */

global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Security.Cryptography;
global using System.Text;

/* global usings for Vinder namespaces here */

global using Vinder.IdentityProvider.Domain.Entities;
global using Vinder.IdentityProvider.Domain.Repositories;
global using Vinder.IdentityProvider.Domain.Filters;
global using Vinder.IdentityProvider.Domain.Filters.Builders;

global using Vinder.IdentityProvider.Infrastructure.Constants;
global using Vinder.IdentityProvider.Infrastructure.Pipelines;

global using Vinder.IdentityProvider.Application.Payloads.Identity;
global using Vinder.IdentityProvider.Application.Services;

global using Vinder.IdentityProvider.Common.Configuration;
global using Vinder.IdentityProvider.Common.Errors;
global using Vinder.IdentityProvider.Common.Results;

global using SecurityToken = Vinder.IdentityProvider.Domain.Entities.SecurityToken;

/* global usings for third-party namespaces here */

global using MongoDB.Driver;
global using MongoDB.Bson;
global using MongoDB.Bson.Serialization;