global using Identity.API.Entities;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using System.ComponentModel.DataAnnotations;
global using MediatR;
global using Identity.API.Infrastructure.Repositories;
global using System.Net;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.Filters;
global using System.Text.Json.Serialization;
global using Identity.API.Application.Commands.UserCommand;
global using Identity.API.Filters;
global using Identity.API.Infrastructure;
global using Microsoft.EntityFrameworkCore.Diagnostics;
global using Serilog;
global using Identity.API.Infrastructure.EntityConfigurations;
global using System.Text;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.IdentityModel.Tokens;
global using Identity.API.Infrastructure.Services;
global using System.IdentityModel.Tokens.Jwt;
global using System.Security.Claims;
global using Microsoft.OpenApi.Models;