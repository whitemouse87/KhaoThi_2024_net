using DotSwashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace khaothi_2024_net_server.Infrastructure.Security
{
    public class SwaggerExcludeFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (schema?.Properties == null || context.Type == null)
                return;

            var excludedProperties = context.Type.GetProperties()
                .Where(t => t.GetCustomAttribute<SwaggerExcludeAttribute>() != null);

            foreach (var excludedProperty in excludedProperties)
            {
                var propertyToRemove = schema.Properties.Keys
                    .SingleOrDefault(x => x.ToLower() == excludedProperty.Name.ToLower());

                if (propertyToRemove != null)
                    schema.Properties.Remove(propertyToRemove);
            }
        }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public class SwaggerExcludeAttribute : Attribute
    {
    }
}
