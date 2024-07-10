using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ChatAppAPI.HealthCheck
{
    public class DatabaseHealthCheck(IConfiguration configuration) : IHealthCheck
    {
        private readonly string? _connString = configuration.GetConnectionString("DefaultConnectionString");

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using var sqlConnection = new SqlConnection(_connString);

                await sqlConnection.OpenAsync(cancellationToken);

                using var command = sqlConnection.CreateCommand();
                command.CommandText = "SELECT 1";

                await command.ExecuteScalarAsync(cancellationToken);

                return HealthCheckResult.Healthy();
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy(
                    context.Registration.FailureStatus.ToString(),
                    exception: ex);
            }
        }
    }
}
