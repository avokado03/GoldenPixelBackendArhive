using GoldenPixel.Db;
using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;
using LinqToDB.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddLinqToDBContext<GpDbConnection>((provider, options)
			=> options
				.UsePostgreSQL(builder.Configuration.GetConnectionString("GpDb"))
				.UseDefaultLogging(provider));

var environment = builder.Environment.EnvironmentName;
builder.Services.AddControllers();
builder.Configuration.AddJsonFile("appsettings.json", false, true);
builder.Configuration.AddJsonFile($"appsettings.{environment}.json", true);
builder.Configuration.AddEnvironmentVariables();
builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
