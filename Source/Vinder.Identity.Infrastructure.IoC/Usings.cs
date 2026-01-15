/* global usings for System namespaces here */

global using System.Diagnostics.CodeAnalysis;
global using System.Security.Cryptography;

/* global usings for Microsoft namespaces here */

global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Hosting;

/* global usings for Vinder namespaces here */

global using Vinder.Identity.Common.Configuration;
global using Vinder.Identity.Domain.Collections;
global using Vinder.Identity.Domain.Aggregates;

global using Vinder.Identity.Application.Services;
global using Vinder.Identity.Application.Providers;
global using Vinder.Identity.Application.Payloads.Identity;
global using Vinder.Identity.Application.Payloads.Group;
global using Vinder.Identity.Application.Payloads.Permission;
global using Vinder.Identity.Application.Payloads.Tenant;
global using Vinder.Identity.Application.Payloads.User;
global using Vinder.Identity.Application.Payloads.Scope;

global using Vinder.Identity.Application.Validators.Permission;
global using Vinder.Identity.Application.Validators.Group;
global using Vinder.Identity.Application.Validators.Identity;
global using Vinder.Identity.Application.Validators.Tenant;
global using Vinder.Identity.Application.Validators.User;
global using Vinder.Identity.Application.Validators.Scope;
global using Vinder.Identity.Application.Handlers.Identity;

global using Vinder.Identity.Infrastructure.Providers;
global using Vinder.Identity.Infrastructure.Persistence;
global using Vinder.Identity.Infrastructure.Security;

/* global usings for third-party namespaces here */

global using MongoDB.Driver;
global using FluentValidation;
global using FluentValidation.AspNetCore;