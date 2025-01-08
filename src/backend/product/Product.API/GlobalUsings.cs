global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore.Diagnostics;
global using System.Net;
global using System.Text.Json.Serialization;
global using Microsoft.EntityFrameworkCore;
global using Product.Infrastructure;
global using Product.Application.Commands.ProductCommand;
global using Product.Infrastructure.Repositories;
global using Serilog;
global using MediatR;
global using Microsoft.AspNetCore.Mvc.Filters;
global using Product.API.Filters;
//use ProductEntity to handle ambigous between reference and class model
global using ProductEntity = Product.Domain.AggregateModels.ProductAggregate.Product;