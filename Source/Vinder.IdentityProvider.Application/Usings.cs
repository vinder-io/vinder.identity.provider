/* global usings for System namespaces here */

global using System.Text.Json.Serialization;

/* global usings for Vinder namespaces here */

global using Vinder.IdentityProvider.Common.Results;
global using Vinder.IdentityProvider.Common.Errors;
global using Vinder.IdentityProvider.Common.Constants;

global using Vinder.IdentityProvider.Domain.Entities;
global using Vinder.IdentityProvider.Domain.Filters;
global using Vinder.IdentityProvider.Domain.Filters.Builders;
global using Vinder.IdentityProvider.Domain.Repositories;

global using Vinder.IdentityProvider.Application.Payloads.Common;
global using Vinder.IdentityProvider.Application.Payloads.Identity;
global using Vinder.IdentityProvider.Application.Payloads.Group;
global using Vinder.IdentityProvider.Application.Payloads.Permission;
global using Vinder.IdentityProvider.Application.Payloads.Tenant;

global using Vinder.IdentityProvider.Application.Services;
global using Vinder.IdentityProvider.Application.Providers;
global using Vinder.IdentityProvider.Application.Mappers;

/* global usings for third-party namespaces here */

global using FluentValidation;
global using MediatR;