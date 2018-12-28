using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace TodoSPA.Controllers
{
    [Authorize]
    public class WhoAmIController : ApiController
    {
        public async Task<IHttpActionResult> Get()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["AzureSqlDb"].ConnectionString;

            string token = await GetDelegatedADTokenForSqlAsync();

            string me = await GetCurrentSqlUserNameAsync(connectionString, token);
                       
            return Ok(me);
        }

        

        private async Task<string> GetCurrentSqlUserNameAsync(string connectionString, string token)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.AccessToken = token;
                await connection.OpenAsync();

                string group = "ANIVN_AzureADSQLUsers";
                string query = $"SELECT SUSER_SNAME() + (CASE IS_MEMBER('{group}') WHEN 1 THEN ' Is a Member of {group}' ELSE ' Is Not a Member of {group}' END)";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    var result = await cmd.ExecuteScalarAsync();
                    return result as string;
                }
            }
        }
    }
}
