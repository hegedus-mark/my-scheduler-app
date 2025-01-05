using Api.Extensions;
using Api.Infrastructure.Mapping.Scheduling;
using Application;
using Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureProblemDetails();

//Add Layers
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

//AutoMapper
builder.Services.AddAutoMapper(typeof(SchedulingMappingProfile).Assembly);

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
