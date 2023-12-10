using webapi.Database;

namespace webapi.Endpoints
{
    public class UserEndpoint
    {
        private WebApplication app;

        public UserEndpoint(WebApplication app)
        {
            this.app = app;
            app.MapGet("/users", GetAll).WithOpenApi();
            app.MapGet("/user/{guid}", GetGuid).WithOpenApi();
            app.MapPost("/user", PostSave).WithOpenApi();
            app.MapDelete("/user/{guid}", DeleteGuid).WithOpenApi();
        }

        private Response<List<UserEntity>> GetAll(HttpContext context)
        {
            try
            {
                return new Response<List<UserEntity>>(UserEntity.LoadAll());
            }
            catch (Exception ex)
            {
                return new Response<List<UserEntity>>(ex);
            }
        }
        private Response<UserEntity> GetGuid(HttpContext context, Guid guid)
        {
            try
            {
                return new Response<UserEntity>(UserEntity.LoadGuid(guid));
            }
            catch (Exception ex)
            {
                return new Response<UserEntity>(ex);
            }
        }
        private Response<bool> PostSave(HttpContext context, UserEntity value)
        {
            try
            {
                return new Response<bool>(value.Save());
            }
            catch (Exception ex)
            {
                return new Response<bool>(ex);
            }
        }
        private Response<bool> DeleteGuid(HttpContext context, Guid guid)
        {
            try {
                return new Response<bool>(UserEntity.Remove(guid));
            }
            catch (Exception ex)
            {
                return new Response<bool>(ex);
            }
        }
    }
}
