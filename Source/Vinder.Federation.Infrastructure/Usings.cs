global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using System.Security.Cryptography;

global using Microsoft.IdentityModel.Tokens;

global using Vinder.Internal.Infrastructure.Persistence;
global using Vinder.Internal.Infrastructure.Persistence.Pipelines;
global using Vinder.Internal.Essentials.Patterns;

global using Vinder.Federation.Domain.Aggregates;
global using Vinder.Federation.Domain.Errors;
global using Vinder.Federation.Domain.Collections;
global using Vinder.Federation.Domain.Filtering;
global using Vinder.Federation.Domain.Filtering.Builders;

global using Vinder.Federation.Infrastructure.Constants;
global using Vinder.Federation.Infrastructure.Pipelines;

global using Vinder.Federation.Application.Payloads.Identity;
global using Vinder.Federation.Application.Services;
global using Vinder.Federation.Application.Providers;
global using Vinder.Federation.Application.Payloads.Client;

global using SecurityToken = Vinder.Federation.Domain.Aggregates.SecurityToken;

global using MongoDB.Driver;
global using MongoDB.Bson;
global using MongoDB.Bson.Serialization;