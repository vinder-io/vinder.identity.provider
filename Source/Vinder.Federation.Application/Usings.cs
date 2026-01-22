global using System.Text.Json.Serialization;
global using System.Security.Cryptography;

global using Vinder.Internal.Essentials.Patterns;
global using Vinder.Internal.Essentials.Filtering;

global using Vinder.Federation.Domain.Errors;
global using Vinder.Federation.Domain.Policies;
global using Vinder.Federation.Domain.Concepts;
global using Vinder.Federation.Common.Constants;

global using Vinder.Federation.Domain.Aggregates;
global using Vinder.Federation.Domain.Filtering;
global using Vinder.Federation.Domain.Filtering.Builders;
global using Vinder.Federation.Domain.Collections;

global using Vinder.Federation.Application.Payloads.Common;
global using Vinder.Federation.Application.Payloads.Identity;
global using Vinder.Federation.Application.Payloads.Authorization;
global using Vinder.Federation.Application.Payloads.Group;
global using Vinder.Federation.Application.Payloads.Permission;
global using Vinder.Federation.Application.Payloads.Tenant;
global using Vinder.Federation.Application.Payloads.User;
global using Vinder.Federation.Application.Payloads.Scope;
global using Vinder.Federation.Application.Payloads.Client;
global using Vinder.Federation.Application.Payloads.Connect;

global using Vinder.Federation.Application.Services;
global using Vinder.Federation.Application.Providers;
global using Vinder.Federation.Application.Mappers;

global using FluentValidation;
global using Vinder.Dispatcher.Contracts;
