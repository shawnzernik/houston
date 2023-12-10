using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace webapi.Database
{
    [Table("Messages")]
    public class MessageEntity : CopyFrom<MessageEntity>
    {
        [Key]
        public Guid? Guid { get; set; }
        public Guid? ToUser { get; set; }
        public DateTime? Created { get; set; }
        public string? Content { get; set; }

        public void CopyFrom(MessageEntity source)
        {
            this.ToUser = source.ToUser;
            this.Created = source.Created;
            this.Content = source.Content;
        }

        public static List<MessageEntity> LoadAll()
        {
            DatabaseContext db = new DatabaseContext();
            return db.Messages.OrderBy(message => message.Created).ToList();
        }

        public static MessageEntity LoadGuid(Guid guid)
        {
            DatabaseContext db = new DatabaseContext();
            return db.Messages.FirstOrDefault(message => message.Guid == guid);
        }

        public static bool Remove(Guid guid)
        {
            DatabaseContext db = new DatabaseContext();
            MessageEntity tracked = db.Messages.FirstOrDefault(message => message.Guid == guid);

            if (tracked == null)
                return false;

            db.Messages.Remove(tracked);
            return db.SaveChanges() == 1;
        }

        public bool Save()
        {
            DatabaseContext db = new DatabaseContext();
            MessageEntity tracked = db.Messages.FirstOrDefault(message => message.Guid == this.Guid);

            if (tracked != null)
                tracked.CopyFrom(this);
            else
                db.Messages.Add(this);

            return db.SaveChanges() == 1;
        }
    }
}
