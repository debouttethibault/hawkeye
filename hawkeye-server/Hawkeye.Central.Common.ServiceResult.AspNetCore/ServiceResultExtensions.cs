using Microsoft.AspNetCore.Mvc;
using System;

namespace Hawkeye.Central.Common.ServiceResult.AspNetCore
{
    public static class ServiceResultExtensions
    {
        public static IActionResult ToActionResult(this ServiceResult result)
        {
            return result.Type switch
            {
                ServiceResultType.Ok => new OkObjectResult(result),
                ServiceResultType.Unexpected => new BadRequestObjectResult(result),
                ServiceResultType.NotFound => new NotFoundObjectResult(result),
                ServiceResultType.Unauthorized => new UnauthorizedObjectResult(result),
                ServiceResultType.Invalid => new BadRequestObjectResult(result),
                _ => throw new Exception($"Unhandled ServiceResult type {Enum.GetName(result.Type)}"),
            };
        }
    }
}
