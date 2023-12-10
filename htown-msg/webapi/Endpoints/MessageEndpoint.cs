using webapi.Database;

namespace webapi.Endpoints
{
    public class MessageEndpoint
    {
        private WebApplication app;

        public MessageEndpoint(WebApplication app)
        {
            this.app = app;
            app.MapGet("/messages", GetAll).WithOpenApi();
            app.MapGet("/message/{guid}", GetGuid).WithOpenApi();
            app.MapPost("/message", PostSave).WithOpenApi();
            app.MapDelete("/message/{guid}", DeleteGuid).WithOpenApi();
        }

        private Response<List<MessageEntity>> GetAll(HttpContext context)
        {
            try
            {
                return new Response<List<MessageEntity>>(MessageEntity.LoadAll());
            }
            catch (Exception ex)
            {
                return new Response<List<MessageEntity>>(ex);
            }
        }
        private Response<MessageEntity> GetGuid(HttpContext context, Guid guid)
        {
            try
            {
                return new Response<MessageEntity>(MessageEntity.LoadGuid(guid));
            }
            catch (Exception ex)
            {
                return new Response<MessageEntity>(ex);
            }
        }
        private Response<bool> PostSave(HttpContext context, MessageEntity value)
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
            try
            {
                return new Response<bool>(MessageEntity.Remove(guid));
            }
            catch (Exception ex)
            {
                return new Response<bool>(ex);
            }
        }
    }
}
