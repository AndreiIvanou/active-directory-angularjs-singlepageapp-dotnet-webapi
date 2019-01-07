using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace TodoSPA.Controllers
{
    [Authorize]
    public class WhoAmIController : ApiController
    {
        private const string Password = "PR1V+Mok7IErHiKOCAyopJn9HR5yD+Kal3LMpQZA110=";

        public async Task<IHttpActionResult> Get(string group, bool shouldImpersonate)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["AzureSqlDb"].ConnectionString;

            string authorization = HttpContext.Current.Request.Headers["Authorization"];

            string currentUserToken = authorization.Substring("Bearer ".Length);

            string tokenForSql = shouldImpersonate? await GetDelegatedADTokenForSqlAsync(currentUserToken) : await GetADTokenForSqlAsync();

            string me = await GetCurrentSqlUserNameAsync(connectionString, tokenForSql, group);

            var response = new {
                me = me,
                currentUserToken = currentUserToken,
                tokenForSql = tokenForSql
            };

            return Json(response);
        }

        public static async Task<string> GetADTokenForSqlAsync()
        {
            var context = new AuthenticationContext("https://login.windows.net/b41b72d0-4e9f-4c26-8a69-f949f367c91d");

            var credentials = new ClientCredential("ab4df43c-22f8-41b6-80fe-67dd32489591", Password);

            var result = await context.AcquireTokenAsync("https://database.windows.net/", credentials);

            return result.AccessToken;
        }

        public static async Task<string> GetDelegatedADTokenForSqlAsync(string currentUserToken)
        {
            var context = new AuthenticationContext("https://login.windows.net/b41b72d0-4e9f-4c26-8a69-f949f367c91d");

            var credentials = new ClientCredential("ab4df43c-22f8-41b6-80fe-67dd32489591", Password);

            var userAssertion = new UserAssertion(currentUserToken);

            var result = await context.AcquireTokenAsync("https://database.windows.net/", credentials, userAssertion);

            return result.AccessToken;
        }


        private async Task<string> GetCurrentSqlUserNameAsync(string connectionString, string token, string group)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.AccessToken = token;
                await connection.OpenAsync();

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
