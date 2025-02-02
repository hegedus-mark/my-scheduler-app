using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Api.Configuration;

public static class SwaggerConfiguration
{
    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            // Add TimeSpan mapping
            options.MapType<TimeSpan>(
                () =>
                    new OpenApiSchema
                    {
                        Type = "string",
                        Example = new OpenApiString("01:30:00"),
                        Description = "Format: hh:mm:ss or dd.hh:mm:ss",
                    }
            );

            options.SchemaFilter<TimeSpanSchemaFilter>();
        });
        return services;
    }
}

public class TimeSpanSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type == typeof(TimeSpan))
        {
            schema.Type = "string";
            schema.Format = "duration";
            schema.Example = new OpenApiString("01:30:00");
            schema.Description = "Format: hh:mm:ss or dd.hh:mm:ss";
        }
    }
}
