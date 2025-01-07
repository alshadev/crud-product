global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Product.Infrastructure.EntityConfigurations;
global using System.Linq.Expressions;
//use ProductEntity to handle ambigous between reference and class model
global using ProductEntity = Product.Domain.AggregateModels.ProductAggregate.Product;