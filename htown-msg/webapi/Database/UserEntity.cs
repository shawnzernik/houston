using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Database
{
    [Table("Users")]
    public class UserEntity : CopyFrom<UserEntity>
    {
        [Key]
        public Guid? Guid { get; set; }
        public string? Name { get; set; }

        public void CopyFrom(UserEntity source)
        {
            this.Name = source.Name;
        }

        public static List<UserEntity> LoadAll()
        {
            DatabaseContext db = new DatabaseContext();
            return db.Users.OrderBy(user => user.Name).ToList();
        }
        public static UserEntity LoadGuid(Guid guid)
        {
            DatabaseContext db = new DatabaseContext();
            return db.Users.FirstOrDefault(user => user.Guid == guid);
        }
        public static bool Remove(Guid guid)
        {
            DatabaseContext db = new DatabaseContext();
            UserEntity tracked = db.Users.FirstOrDefault(user => user.Guid == guid);

            if (tracked == null)
                return false;

            db.Users.Remove(tracked);
            return db.SaveChanges() == 1;
        }
        public bool Save()
        {
            DatabaseContext db = new DatabaseContext();
            UserEntity tracked = db.Users.FirstOrDefault(user => user.Guid == this.Guid);

            if (tracked != null)
                tracked.CopyFrom(this);
            else
                db.Users.Add(this);

            return db.SaveChanges() == 1;
        }
    }
}
