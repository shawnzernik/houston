using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Database;

[Table("Messages")]
public class MessageEntity : CopyFrom<MessageEntity>
{
    private static readonly Logger logger = new Logger(typeof(MessageEntity));

    [Key]
    public Guid? Guid { get; set; }
    public Guid? ToUser { get; set; }
    public DateTime? Created { get; set; }
    public string? Content { get; set; }

    public void CopyFrom(MessageEntity source)
    {
        logger.Trace("CopyFrom(MessageEntity source)");

        ToUser = source.ToUser;
        Created = source.Created;
        Content = source.Content;
    }

    public static List<MessageEntity> LoadAll()
    {
        logger.Trace("LoadAll()");

        DatabaseContext db = new DatabaseContext();
        return db.Messages.OrderBy(message => message.Created).ToList();
    }

    public static MessageEntity? LoadGuid(Guid guid)
    {
        logger.Trace("LoadGuid(Guid guid)");

        DatabaseContext db = new DatabaseContext();
        return db.Messages.FirstOrDefault(message => message.Guid == guid);
    }

    public static bool Remove(Guid guid)
    {
        logger.Trace("Remove(Guid guid)");

        DatabaseContext db = new DatabaseContext();
        MessageEntity? tracked = db.Messages.FirstOrDefault(message => message.Guid == guid);

        if (tracked == null)
            return false;

        db.Messages.Remove(tracked);
        return db.SaveChanges() == 1;
    }

    public bool Save()
    {
        logger.Trace("Save()");

        DatabaseContext db = new DatabaseContext();
        MessageEntity? tracked = db.Messages.FirstOrDefault(message => message.Guid == Guid);

        if (tracked != null)
            tracked.CopyFrom(this);
        else
            db.Messages.Add(this);

        return db.SaveChanges() == 1;
    }
}
