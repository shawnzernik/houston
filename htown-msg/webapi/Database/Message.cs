namespace webapi.Database
{
    public class Message
    {
        public Guid Guid { get; set; }
        public Guid ToUser { get; set; }
        public DateTime Created { get; set; }
        public string? Content { get; set; }
    }
}
