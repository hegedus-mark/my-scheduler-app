using System.Text.Json.Serialization;
using Api.Configuration;
using Api.Controllers.DraftTasks;
using Application.DraftTasks;
using Application.DraftTasks.CreateDraftTask;
using Application.DraftTasks.DeleteDraftTask;
using Application.DraftTasks.GetAllDraftTasks;
using Application.DraftTasks.UpdateDraftTask;
using dotenv.net;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

DotEnv.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder
    .Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });

var corsOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "DefaultPolicy",
        builder => { builder.WithOrigins(corsOrigins).AllowAnyHeader().AllowAnyMethod().AllowCredentials(); }
    );
});


builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureProblemDetails();

//DraftTaskServices
builder.Services.AddScoped<ICreateDraftTaskService, CreateDraftTaskService>();
builder.Services.AddScoped<IUpdateDraftTaskService, UpdateDraftTaskService>();
builder.Services.AddScoped<IGetAllDraftTasksService, GetAllDraftTasksService>();
builder.Services.AddScoped<IDeleteDraftTaskService, DeleteDraftTaskService>();
builder.Services.AddScoped<IDraftTaskRepository, DraftTaskRepository>();

//AutoMapper
builder.Services.AddAutoMapper(typeof(DraftTaskMappingProfile).Assembly);

//Database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(connectionString));

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