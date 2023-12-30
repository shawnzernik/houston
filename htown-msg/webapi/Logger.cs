using Microsoft.Extensions.Logging.Console;

namespace webapi;

public class Logger
{
	private ILogger logger;

	public Logger(Type namedScope)
	{
		Action<ILoggingBuilder> builder = (builder) =>
		{
			builder.AddConsole();
			builder.SetMinimumLevel(LogLevel.Trace); /* THIS IS CRITICAL TO GET TRACES */
		};
		ILoggerFactory factory = LoggerFactory.Create(builder);
		logger = factory.CreateLogger(namedScope);

		Trace("Logger(Type namedScope = " + namedScope.FullName + ")");
	}

	public Guid? Corelation
	{
		get; set;
	}

	private string PrepareMessage(string message)
	{
		string msg = "";
		if (Corelation != null)
			msg += Corelation.ToString() + " - ";
		msg += message;
		return msg;
	}

	public void Critical(string message) { logger.LogCritical(PrepareMessage(message)); }
	public void Debug(string message) { logger.LogDebug(PrepareMessage(message)); }
	public void Error(string message) { logger.LogError(PrepareMessage(message)); }
	public void Information(string message) { logger.LogInformation(PrepareMessage(message)); }
	public void Trace(string message) { logger.LogTrace(PrepareMessage(message)); }
	public void Warning(string message) { logger.LogWarning(PrepareMessage(message)); }

	public void Exception(Exception ex)
	{
		logger.LogError(PrepareMessage(ex.ToString()));
	}
}