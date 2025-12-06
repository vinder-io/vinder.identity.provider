/* global usings for System namespaces here */

global using System.Text.Json.Serialization;
global using System.Security.Cryptography;

/* global usings for Vinder namespaces here */

global using Vinder.Internal.Essentials.Patterns;
global using Vinder.Internal.Essentials.Filters;

global using Vinder.IdentityProvider.Domain.Errors;
global using Vinder.IdentityProvider.Common.Constants;

global using Vinder.IdentityProvider.Domain.Entities;
global using Vinder.IdentityProvider.Domain.Filtering;
global using Vinder.IdentityProvider.Domain.Filtering.Builders;
global using Vinder.IdentityProvider.Domain.Repositories;

global using Vinder.IdentityProvider.Application.Payloads.Common;
global using Vinder.IdentityProvider.Application.Payloads.Identity;
global using Vinder.IdentityProvider.Application.Payloads.Group;
global using Vinder.IdentityProvider.Application.Payloads.Permission;
global using Vinder.IdentityProvider.Application.Payloads.Tenant;
global using Vinder.IdentityProvider.Application.Payloads.User;
global using Vinder.IdentityProvider.Application.Payloads.Scope;
global using Vinder.IdentityProvider.Application.Payloads.Client;
global using Vinder.IdentityProvider.Application.Payloads.OpenID;

global using Vinder.IdentityProvider.Application.Services;
global using Vinder.IdentityProvider.Application.Providers;
global using Vinder.IdentityProvider.Application.Mappers;

/* global usings for third-party namespaces here */

global using FluentValidation;
global using MediatR;