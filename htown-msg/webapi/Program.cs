using webapi.Endpoints;

namespace webapi;

public class Program
{
    private static readonly Logger logger = new Logger(typeof(Program));

    public static int Main(string[] args)
    {
        logger.Trace("Main(string[] args)");

        logger.Information("Houston Message Board");

        logger.Information("Setting up web application");
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        logger.Information("Building web application");
        var app = builder.Build();

        logger.Information("Adding Swagger");
        app.UseSwagger();
        app.UseSwaggerUI();

        logger.Information("Adding wwwroot");
        app.UseDefaultFiles();
        app.UseStaticFiles();

        logger.Information("Adding endpoints");
        var messageEndpoint = new MessageEndpoint(app);
        var userEndpoint = new UserEndpoint(app);

        logger.Information("Running website");
        app.Run();
        return 0;
    }
}