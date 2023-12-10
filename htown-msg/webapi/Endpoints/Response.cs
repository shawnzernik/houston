namespace webapi.Endpoints
{
    public class Response<T>
    {
        public Response(Exception ex) 
        {
            this.Error = ex.Message;
            this.Trace = ex.StackTrace;

            if (ex.InnerException != null)
                this.Inner = new Response<T>(ex.InnerException);
        }
        public Response(T content)
        {
            this.Content = content;
        }

        public T? Content { get; set; }
        public string? Error { get; set; }
        public string? Trace { get; set; }
        public Response<T>? Inner { get; set; }
    }
}
