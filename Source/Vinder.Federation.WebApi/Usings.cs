global using System.Diagnostics.CodeAnalysis;
global using System.Text.Json;
global using System.Text.RegularExpressions;
global using System.Net.Mime;
global using System.Security.Claims;

global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.ModelBinding;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.Extensions.Caching.Memory;
global using Microsoft.IdentityModel.Tokens;
global using Microsoft.OpenApi.Models;

global using Vinder.Federation.Domain.Aggregates;
global using Vinder.Federation.Domain.Filtering;
global using Vinder.Federation.Domain.Collections;

global using Vinder.Federation.Common.Constants;
global using Vinder.Federation.Common.Configuration;
global using Vinder.Federation.Domain.Errors;

global using Vinder.Federation.Application.Payloads.Group;
global using Vinder.Federation.Application.Payloads.Identity;
global using Vinder.Federation.Application.Payloads.Authorization;
global using Vinder.Federation.Application.Payloads.Permission;
global using Vinder.Federation.Application.Payloads.Tenant;
global using Vinder.Federation.Application.Payloads.User;
global using Vinder.Federation.Application.Payloads.Scope;
global using Vinder.Federation.Application.Payloads.Connect;

global using Vinder.Federation.Application.Providers;
global using Vinder.Federation.Application.Services;

global using Vinder.Federation.Infrastructure.IoC.Extensions;

global using Vinder.Federation.WebApi.Extensions;
global using Vinder.Federation.WebApi.Middlewares;
global using Vinder.Federation.WebApi.Attributes;
global using Vinder.Federation.WebApi.Binders;
global using Vinder.Federation.WebApi.Providers;
global using Vinder.Federation.WebApi.Constants;

global using Vinder.Internal.Essentials.Utilities;

global using Vinder.Dispatcher.Contracts;
global using Scalar.AspNetCore;
