namespace webapi.Endpoints;

public class Response<T>
{
    private static readonly Logger logger = new Logger(typeof(Response<T>));

    public Response(Exception ex) 
    {
        logger.Trace("Response(Exception ex)");
        logger.Exception(ex);

        Error = ex.Message;
        Trace = ex.StackTrace;

        if (ex.InnerException != null)
            Inner = new Response<T>(ex.InnerException);
    }
    public Response(T? content)
    {
        logger.Trace("Response(T content)");

        Content = content;
    }

    public T? Content { get; set; }
    public string? Error { get; set; }
    public string? Trace { get; set; }
    public Response<T>? Inner { get; set; }
}
