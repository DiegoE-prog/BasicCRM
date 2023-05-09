using BasicCRM.Data;
using BasicCRM.Data.Entities;
using BasicCRM.Data.Repository.Interfaces;
using BasicCRM.Data.Repository;
using Microsoft.Extensions.Configuration;
using BasicCRM.Business.Services.Interfaces;
using BasicCRM.Business.Services;
using BasicCRM.Business;
using BasicCRM.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddBusinessServices();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection(nameof(DatabaseSettings)));

builder.Services.AddScoped<IRepository<Address>, AddressRepository>();
builder.Services.AddScoped<IRepository<Client>,  ClientRepository>();

builder.Services.AddScoped<GlobalHandleExceptionMiddleware>();

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

app.UseMiddleware<GlobalHandleExceptionMiddleware>();

app.Run();
