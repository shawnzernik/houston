﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
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
            app.MapPost("/user", Save).WithOpenApi();
            app.MapDelete("/user/{guid}", Remove).WithOpenApi();
        }

        private bool Remove(HttpContext context, Guid guid)
        {
            DatabaseContext db = new DatabaseContext();

            User tracked = null;
            try { tracked = db.Users.Single(user => user.Guid == guid); }
            catch (InvalidOperationException ex)
            {
                if (!ex.Message.Contains("Sequence contains no elements"))
                    throw;
            }

            if (tracked == null)
                return false;

            db.Users.Remove(tracked);
            return db.SaveChanges() == 1;
        }

        private bool Save(HttpContext context, User value)
        {
            if (value.Guid == null)
                return Insert(context, value);
            else
                return Update(context, value);
        }

        private bool Insert(HttpContext context, User value)
        {
            if (value.Guid == null)
                value.Guid = Guid.NewGuid();

            DatabaseContext db = new DatabaseContext();
            db.Users.Add(value);
            return db.SaveChanges() == 1;
        }

        private bool Update(HttpContext context, User value)
        {
            DatabaseContext db = new DatabaseContext();

            User tracked = null;
            try { tracked = db.Users.Single(user => user.Guid == value.Guid); }
            catch(InvalidOperationException ex) 
            {
                if (!ex.Message.Contains("Sequence contains no elements"))
                    throw;
            }

            if (tracked == null)
                return Insert(context, value);

            tracked.CopyFrom(value);
            return db.SaveChanges() == 1;
        }

        private List<User> GetAll(HttpContext context)
        {
            DatabaseContext db = new DatabaseContext();
            return db.Users.OrderBy(user => user.Name).ToList();
        }
    }
}
