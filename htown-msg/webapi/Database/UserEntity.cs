using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Database;

[Table("Users")]
public class UserEntity : CopyFrom<UserEntity>
{
    private static readonly Logger logger = new Logger(typeof(UserEntity));

    [Key]
    public Guid? Guid { get; set; }
    public string? Name { get; set; }

    public void CopyFrom(UserEntity source)
    {
        logger.Trace("CopyFrom(UserEntity source)");

        Name = source.Name;
    }

    public static List<UserEntity> LoadAll()
    {
        logger.Trace("LoadAll()");

        DatabaseContext db = new DatabaseContext();
        return db.Users.OrderBy(user => user.Name).ToList();
    }
    public static UserEntity? LoadGuid(Guid guid)
    {
        logger.Trace("LoadGuid(Guid guid)");

        DatabaseContext db = new DatabaseContext();
        return db.Users.FirstOrDefault(user => user.Guid == guid);
    }
    public static bool Remove(Guid guid)
    {
        logger.Trace("Remove(Guid guid)");

        DatabaseContext db = new DatabaseContext();
        UserEntity? tracked = db.Users.FirstOrDefault(user => user.Guid == guid);

        if (tracked == null)
            return false;

        db.Users.Remove(tracked);
        return db.SaveChanges() == 1;
    }
    public bool Save()
    {
        logger.Trace("Save()");

        DatabaseContext db = new DatabaseContext();
        UserEntity? tracked = db.Users.FirstOrDefault(user => user.Guid == Guid);

        if (tracked != null)
            tracked.CopyFrom(this);
        else
            db.Users.Add(this);

        return db.SaveChanges() == 1;
    }
}
