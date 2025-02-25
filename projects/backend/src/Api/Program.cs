using System.Text.Json.Serialization;
using Api.Configuration;
using Api.Controllers.DraftTasks;
using Application.DraftTasks.CreateDraftTask;
using dotenv.net;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder
    .Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

var corsOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "DefaultPolicy",
        builder =>
        {
            builder.WithOrigins(corsOrigins).AllowAnyHeader().AllowAnyMethod().AllowCredentials();
        }
    );
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureProblemDetails();

//Services
builder.Services.AddScoped<ICreateDraftTaskService, CreateDraftTaskService>();

//AutoMapper
builder.Services.AddAutoMapper(typeof(DraftTaskMappingProfile).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("DefaultPolicy");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
