using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SsoMultipleProviders.Common.Swagger
{
    public class CommandQuerySchemaFilter
    {
        public static string ApplyCustomSchema(Type type)
        {
            if (type.Name.EndsWith("Command") || type.Name.EndsWith("Query"))
            {
                // Check if there is a parent class
                var declaringType = type.DeclaringType;
                if (declaringType != null)
                {
                    // Combine the declaring (parent) class name with the nested class name
                    return $"{declaringType.Name}{type.Name}";
                }
            }

            // For other types, use the full name of the type (including namespace) to ensure uniqueness
            // Replace '+' with '.' for nested classes
            return type.Name.Replace("Dto", "");
        }
    }
}
