using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace Authorization.API.Builders;

public static class HealthCheckOptionsBuilder
{
    public static HealthCheckOptions Build()
    {
        return new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";
                var response = new
                {
                    status = report.Status.ToString(),
                    checks = report.Entries.Select(x => new
                    {
                        name = x.Key,
                        status = x.Value.Status.ToString(),
                        description = x.Value.Description
                    })
                };
                await context.Response.WriteAsJsonAsync(response);
            }
        };
    }
} 