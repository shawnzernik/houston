using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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
            app.MapPost("/message", Save).WithOpenApi();
            app.MapDelete("/message/{guid}", Remove).WithOpenApi();
        }

        private bool Remove(HttpContext context, Guid guid)
        {
            DatabaseContext db = new DatabaseContext();

            Message tracked = null;
            try { tracked = db.Messages.Single(m => m.Guid == guid); }
            catch(InvalidOperationException ex)
            {
                if (!ex.Message.Contains("Sequence contained no elements"))
                    throw ex;
            }

            if (tracked == null)
                return false;

            db.Messages.Remove(tracked);
            return db.SaveChanges() == 1;
        }

        private bool Save(HttpContext context, Message value)
        {
            if (value.Guid == null)
                return Insert(context, value);
            else
                return Update(context, value);
        }

        private bool Insert(HttpContext context, Message value)
        {
            if(value.Guid == null)
                value.Guid = Guid.NewGuid();

            DatabaseContext db = new DatabaseContext();
            db.Messages.Add(value);
            return db.SaveChanges() == 1;
        }

        private bool Update(HttpContext context, Message value)
        {
            DatabaseContext db = new DatabaseContext();

            Message tracked = null;
            try { tracked = db.Messages.Single(message => message.Guid == value.Guid);  }
            catch(InvalidOperationException ex)
            {
                if (!ex.Message.Contains("Sequence contains no elements"))
                    throw ex;
            }

            if (tracked == null)
                return Insert(context, value);

            tracked.CopyFrom(value);
            return db.SaveChanges() == 1;
        }

        private List<Message> GetAll(HttpContext context)
        {
            DatabaseContext db = new DatabaseContext();
            return db.Messages.OrderBy(message => message.Created).ToList();
        }
    }
}
