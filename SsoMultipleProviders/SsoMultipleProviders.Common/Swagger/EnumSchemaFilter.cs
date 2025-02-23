using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsoMultipleProviders.Common.Swagger
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;
            if (type.IsEnum)
            {
                schema.Enum.Clear();
                Enum.GetNames(type).ToList()
                    .ForEach(name => schema.Enum.Add(new OpenApiString(name)));
            }
            else if (Nullable.GetUnderlyingType(type)?.IsEnum == true)
            {
                var enumType = Nullable.GetUnderlyingType(type);
                schema.Enum.Clear();
                if (enumType is not null)
                {
                    Enum.GetNames(enumType).ToList()
                    .ForEach(name => schema.Enum.Add(new OpenApiString(name)));
                }
            }
        }
    }
}
