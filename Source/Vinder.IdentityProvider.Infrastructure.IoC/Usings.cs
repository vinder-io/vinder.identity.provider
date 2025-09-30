/* global usings for System namespaces here */

global using System.Diagnostics.CodeAnalysis;
global using System.Security.Cryptography;

/* global usings for Microsoft namespaces here */

global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Hosting;

/* global usings for Vinder namespaces here */

global using Vinder.IdentityProvider.Common.Configuration;
global using Vinder.IdentityProvider.Domain.Repositories;
global using Vinder.IdentityProvider.Domain.Entities;

global using Vinder.IdentityProvider.Application.Services;
global using Vinder.IdentityProvider.Application.Providers;
global using Vinder.IdentityProvider.Application.Payloads.Identity;
global using Vinder.IdentityProvider.Application.Payloads.Group;
global using Vinder.IdentityProvider.Application.Payloads.Permission;
global using Vinder.IdentityProvider.Application.Payloads.Tenant;
global using Vinder.IdentityProvider.Application.Payloads.User;
global using Vinder.IdentityProvider.Application.Payloads.Scope;

global using Vinder.IdentityProvider.Application.Validators.Permission;
global using Vinder.IdentityProvider.Application.Validators.Group;
global using Vinder.IdentityProvider.Application.Validators.Identity;
global using Vinder.IdentityProvider.Application.Validators.Tenant;
global using Vinder.IdentityProvider.Application.Validators.User;
global using Vinder.IdentityProvider.Application.Validators.Scope;
global using Vinder.IdentityProvider.Application.Handlers.Identity;

global using Vinder.IdentityProvider.Infrastructure.Providers;
global using Vinder.IdentityProvider.Infrastructure.Repositories;
global using Vinder.IdentityProvider.Infrastructure.Security;

/* global usings for third-party namespaces here */

global using MongoDB.Driver;
global using FluentValidation;
global using FluentValidation.AspNetCore;