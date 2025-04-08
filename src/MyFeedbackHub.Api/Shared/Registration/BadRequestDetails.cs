﻿using Microsoft.AspNetCore.Http.Features;
using System.Diagnostics;

namespace MyFeedbackHub.Api.Shared.Registration;

public static class BadRequestDetails
{
    internal static IServiceCollection AddBadRequestDetails(this IServiceCollection services)
    {
        // https://www.rfc-editor.org/rfc/rfc9457
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance =
                    $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

                Activity? activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
                context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
            };
        });

        return services;
    }
}
