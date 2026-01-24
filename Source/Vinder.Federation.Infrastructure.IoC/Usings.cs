global using System.Diagnostics.CodeAnalysis;
global using System.Security.Cryptography;

global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Hosting;

global using Vinder.Federation.Common.Configuration;
global using Vinder.Federation.Domain.Collections;
global using Vinder.Federation.Domain.Aggregates;
global using Vinder.Federation.Domain.Policies;

global using Vinder.Federation.Application.Services;
global using Vinder.Federation.Application.Providers;
global using Vinder.Federation.Application.Payloads.Identity;
global using Vinder.Federation.Application.Payloads.Authorization;
global using Vinder.Federation.Application.Payloads.Group;
global using Vinder.Federation.Application.Payloads.Permission;
global using Vinder.Federation.Application.Payloads.Tenant;
global using Vinder.Federation.Application.Payloads.User;
global using Vinder.Federation.Application.Payloads.Scope;

global using Vinder.Federation.Application.Validators.Permission;
global using Vinder.Federation.Application.Validators.Group;
global using Vinder.Federation.Application.Validators.Identity;
global using Vinder.Federation.Application.Validators.Authorization;
global using Vinder.Federation.Application.Validators.Tenant;
global using Vinder.Federation.Application.Validators.User;
global using Vinder.Federation.Application.Validators.Scope;

global using Vinder.Federation.Application.Handlers.Identity;
global using Vinder.Federation.Application.Policies;

global using Vinder.Federation.Infrastructure.Providers;
global using Vinder.Federation.Infrastructure.Persistence;
global using Vinder.Federation.Infrastructure.Security;
global using Vinder.Dispatcher.Extensions;

global using MongoDB.Driver;
global using FluentValidation;
global using FluentValidation.AspNetCore;
