/* global usings for System namespaces here */

global using System.Diagnostics.CodeAnalysis;
global using System.Security.Cryptography;
global using System.Text;
global using System.Text.Json;
global using System.Text.RegularExpressions;
global using System.Net.Mime;

/* global usings for Microsoft namespaces here */

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.ModelBinding;
global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.IdentityModel.Tokens;

/* global usings for Vinder namespaces here */

global using Vinder.IdentityProvider.Domain.Entities;
global using Vinder.IdentityProvider.Domain.Filters.Builders;
global using Vinder.IdentityProvider.Domain.Repositories;

global using Vinder.IdentityProvider.Common.Constants;
global using Vinder.IdentityProvider.Common.Configuration;
global using Vinder.IdentityProvider.Common.Errors;

global using Vinder.IdentityProvider.Application.Payloads.Identity;
global using Vinder.IdentityProvider.Application.Payloads.Group;
global using Vinder.IdentityProvider.Application.Payloads.Permission;

global using Vinder.IdentityProvider.Application.Providers;
global using Vinder.IdentityProvider.Application.Services;

global using Vinder.IdentityProvider.Infrastructure.IoC.Extensions;
global using Vinder.IdentityProvider.WebApi.Extensions;
global using Vinder.IdentityProvider.WebApi.Middlewares;
global using Vinder.IdentityProvider.WebApi.Attributes;
global using Vinder.IdentityProvider.WebApi.Binders;


/* global usings for third-party namespaces here */

global using MediatR;
