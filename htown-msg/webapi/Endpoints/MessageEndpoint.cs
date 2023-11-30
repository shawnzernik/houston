

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using webapi.Pocos;

namespace webapi.Endpoints
{
    public class MessageEndpoint
    {
        public MessageEndpoint(WebApplication app)
        {
            app.MapGet("/messages", GetAll).WithOpenApi();
            app.MapPost("/message", Save).WithOpenApi();
            app.MapDelete("/message/{guid}", Remove).WithOpenApi();
        }

        private bool Remove(HttpContext context, Guid guid)
        {
            return false;
        }

        private bool Save(HttpContext context, Message value)
        {
            return false;
        }

        private List<Message> GetAll(HttpContext context)
        {
            return new List<Message>();
        }
    }
}
