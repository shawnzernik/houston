using System.ComponentModel.DataAnnotations;

namespace webapi.Database
{
    public class Message : CopyFrom<Message>
    {
        [Key]
        public Guid? Guid { get; set; }
        public Guid? ToUser { get; set; }
        public DateTime? Created { get; set; }
        public string? Content { get; set; }

        public void CopyFrom(Message source)
        {
            this.ToUser = source.ToUser;
            this.Created = source.Created;
            this.Content = source.Content;
        }
    }
}
