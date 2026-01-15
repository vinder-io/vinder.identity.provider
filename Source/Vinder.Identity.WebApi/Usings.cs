/* global usings for System namespaces here */

global using System.Diagnostics.CodeAnalysis;
global using System.Text.Json;
global using System.Text.RegularExpressions;
global using System.Net.Mime;

/* global usings for Microsoft namespaces here */

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.ModelBinding;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;

/* global usings for Vinder namespaces here */

global using Vinder.Identity.Domain.Aggregates;
global using Vinder.Identity.Domain.Filtering.Builders;
global using Vinder.Identity.Domain.Collections;

global using Vinder.Identity.Common.Constants;
global using Vinder.Identity.Common.Configuration;
global using Vinder.Identity.Domain.Errors;

global using Vinder.Identity.Application.Payloads.Group;
global using Vinder.Identity.Application.Payloads.Identity;
global using Vinder.Identity.Application.Payloads.Permission;
global using Vinder.Identity.Application.Payloads.Tenant;
global using Vinder.Identity.Application.Payloads.User;
global using Vinder.Identity.Application.Payloads.Scope;
global using Vinder.Identity.Application.Payloads.OpenID;

global using Vinder.Identity.Application.Providers;
global using Vinder.Identity.Application.Services;

global using Vinder.Identity.Infrastructure.IoC.Extensions;

global using Vinder.Identity.WebApi.Extensions;
global using Vinder.Identity.WebApi.Middlewares;
global using Vinder.Identity.WebApi.Attributes;
global using Vinder.Identity.WebApi.Binders;
global using Vinder.Identity.WebApi.Constants;

global using Vinder.Internal.Essentials.Utilities;

/* global usings for third-party namespaces here */

global using Vinder.Dispatcher.Contracts;
global using Scalar.AspNetCore;
