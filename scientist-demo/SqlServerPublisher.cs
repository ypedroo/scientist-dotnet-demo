using System;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GitHub;

namespace scientist_demo
{
    class SqlServerPublisher : IResultPublisher
    {
        //private readonly IConfiguration _configuration;
        private readonly ConnectionStrings _connectionStrings;

        public SqlServerPublisher(ConnectionStrings connectionStrings)
        {
            _connectionStrings = connectionStrings;
        }

        public Task Publish<T, TClean>(Result<T, TClean> result)
        {
            using var connection = new SqlConnection(_connectionStrings.ExperimentDb);

            string sql = "INSERT INTO master.dbo.ExperimentsResults (Date, AppVersion, Name, IsMatch, ControlValue, CandidateValue, ControlDurationMs, CandidateDurationMs, Context) VALUES (@Date, @AppVersion, @Name, @IsMatch, @ControlValue, @CandidateValue, @ControlDurationMs, @CandidateDurationMs, @Context)";

            connection.Open();

            try
            {
                using SqlCommand insert = new SqlCommand(sql, connection);
                insert.Parameters.AddWithValue("@Date", DateTime.Now);
                insert.Parameters.AddWithValue("@AppVersion", Assembly.GetExecutingAssembly().GetName().Version.ToString());
                insert.Parameters.AddWithValue("@Name", result.ExperimentName);
                insert.Parameters.AddWithValue("@IsMatch", result.Matched);
                insert.Parameters.AddWithValue("@ControlValue", result.Control.Value);
                insert.Parameters.AddWithValue("@ControlDurationMs", result.Control.Duration.TotalMilliseconds);

                if (result.Candidates.Any())
                {
                    insert.Parameters.AddWithValue("@CandidateValue", result.Candidates[0].Value);
                    insert.Parameters.AddWithValue("@CandidateDurationMs", result.Candidates[0].Duration.TotalMilliseconds);
                }

                if (result.Contexts.Any())
                    insert.Parameters.AddWithValue("@Context", result.Contexts.First().Value);

                insert.ExecuteNonQuery();

                return Task.FromResult(0);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
    }
}