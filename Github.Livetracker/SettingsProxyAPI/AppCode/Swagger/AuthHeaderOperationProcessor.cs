using Microsoft.AspNetCore.Authorization;
using NJsonSchema;
using NSwag;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using System.Linq;

namespace SettingsProxyAPI.AppCode.Swagger
{
    public class AuthHeaderOperationProcessor : IOperationProcessor
    {
        public bool Process(OperationProcessorContext context)
        {
            if (context.MethodInfo.GetCustomAttributes(false)
                .Any(atr => atr is AllowAnonymousAttribute))
            {
                return true;
            }
            context.OperationDescription.Operation.Parameters.Add(
                new OpenApiParameter()
                {
                    Name = "Authentication",
                    Kind = OpenApiParameterKind.Header,
                    Type = JsonObjectType.String,
                    IsRequired = true,
                    Description = "Authentication token",
                });
            return true;
        }
    }
}
