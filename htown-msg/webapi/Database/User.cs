using System.ComponentModel.DataAnnotations;

namespace webapi.Database
{
    public class User : CopyFrom<User>
    {
        [Key]
        public Guid? Guid { get; set; }
        public string? Name { get; set; }

        public void CopyFrom(User source)
        {
            this.Name = source.Name;
        }
    }
}
