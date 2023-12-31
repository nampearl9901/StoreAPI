﻿using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StoreAPI.Model;
using StoreAPI.Services;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication().AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSetting:Token").Value!))
    };
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", builder =>
    {
        builder
            .AllowAnyOrigin() // Cho phép tất cả nguồn
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
//
builder.Services.Configure<DatabaseSettings>(
    builder.Configuration.GetSection("MongoDB")
    );
//
builder.Services.AddTransient<ICategoryServices, CategoryServices>();
//
builder.Services.AddTransient<IProductsServices, ProductsServices>();
//
builder.Services.AddTransient<IBrandServices, BrandServices>();
//
builder.Services.AddTransient<IUservices, UserServices>();

var app = builder.Build();

//
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAnyOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
