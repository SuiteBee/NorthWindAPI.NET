using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace NorthWindAPI.Config.Swagger
{
    public class RemoveSchemaFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            foreach (KeyValuePair<string, OpenApiSchema> item in swaggerDoc.Components.Schemas)
            {
                swaggerDoc.Components.Schemas.Remove(item.Key);
            }
        }
    }
}
