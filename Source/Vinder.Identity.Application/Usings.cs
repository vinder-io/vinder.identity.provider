/* global usings for System namespaces here */

global using System.Text.Json.Serialization;
global using System.Security.Cryptography;

/* global usings for Vinder namespaces here */

global using Vinder.Internal.Essentials.Patterns;
global using Vinder.Internal.Essentials.Filters;

global using Vinder.Identity.Domain.Errors;
global using Vinder.Identity.Common.Constants;

global using Vinder.Identity.Domain.Entities;
global using Vinder.Identity.Domain.Filtering;
global using Vinder.Identity.Domain.Filtering.Builders;
global using Vinder.Identity.Domain.Repositories;

global using Vinder.Identity.Application.Payloads.Common;
global using Vinder.Identity.Application.Payloads.Identity;
global using Vinder.Identity.Application.Payloads.Group;
global using Vinder.Identity.Application.Payloads.Permission;
global using Vinder.Identity.Application.Payloads.Tenant;
global using Vinder.Identity.Application.Payloads.User;
global using Vinder.Identity.Application.Payloads.Scope;
global using Vinder.Identity.Application.Payloads.Client;
global using Vinder.Identity.Application.Payloads.OpenID;

global using Vinder.Identity.Application.Services;
global using Vinder.Identity.Application.Providers;
global using Vinder.Identity.Application.Mappers;

/* global usings for third-party namespaces here */

global using FluentValidation;
global using MediatR;