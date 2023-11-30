

using webapi.Pocos;

namespace webapi.Endpoints
{
    public class UserEndpoint
    {

        public UserEndpoint(WebApplication app)
        {
            app.MapGet("/users", GetAll).WithOpenApi();
            app.MapPost("/user", Save).WithOpenApi();
            app.MapDelete("/user/{guid}", Remove).WithOpenApi();
        }

        private bool Remove(HttpContext context, Guid guid)
        {
            return false;
        }

        private bool Save(HttpContext context, User value)
        {
            return false;
        }

        private List<User> GetAll(HttpContext context)
        {
            return new List<User>();
        }
    }
}
