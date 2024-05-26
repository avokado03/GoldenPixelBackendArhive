using GoldenPixel.Common.Middleware.Configurations;
using GoldenPixel.Db;
using LinqToDB;
using LinqToDB.AspNet;
using LinqToDB.AspNet.Logging;
using NLog;
using NLog.Extensions.Logging;
using NLog.Web;
using System.Reflection;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;
using GoldenPixelBackend.Mail;

var builder = WebApplication.CreateBuilder(args);

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
	builder.Services.AddLinqToDBContext<GpDbConnection>((provider, options)
				=> options
					.UsePostgreSQL(builder.Configuration.GetConnectionString("DbGp"))
					.UseDefaultLogging(provider));

	var environment = builder.Environment.EnvironmentName;
	builder.Services.AddControllers();
	builder.Configuration.AddJsonFile("appsettings.json", false, true);
	builder.Configuration.AddJsonFile($"appsettings.{environment}.json", true);
	builder.Configuration.AddEnvironmentVariables();
	builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly());

	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddSwaggerGen(c => SwaggerMiddlewareConfigurations.ConfigureSwaggerGenOptions(c));

	builder.Services.AddLogging(logger =>
	{
		logger.ClearProviders();
		logger.SetMinimumLevel(LogLevel.Information);
	});

	builder.Services.AddSingleton<ILoggerProvider, NLogLoggerProvider>();

	string? specificOrigins = "SpecificOrigins";
	builder.Services.AddCors(options =>
	{
		options.AddPolicy(name: specificOrigins,
		policy =>
		{
			policy
			.WithOrigins(
				"http://localhost:8080",
				"https://localhost:8080",
				"http://localhost:80",
				"https://localhost:80",
				"https://golden-pixel.kz"
				)
			.SetIsOriginAllowedToAllowWildcardSubdomains()
			.AllowCredentials()
			.WithMethods("POST", "PUT", "DELETE", "OPTIONS")
			.WithHeaders("Origin", "X-Requested-With", "Content-Type", "Accept", "Authorization");
		});
	});

    builder.Services.ConfigureMailKitService(builder.Configuration);

    var app = builder.Build();

//#if DEBUG
    app.UseSwagger(c =>
	{
		c.RouteTemplate = "api/swagger/{documentName}/swagger.json";
	});

	app.UseSwaggerUI(c => SwaggerMiddlewareConfigurations.GetSwaggerUIOptions(c));
//#endif

    app.UseHttpsRedirection();

	app.UseAuthorization();
	app.UseCors(specificOrigins);

	app.MapControllers();

	app.Run();
}
catch(Exception ex)
{
	logger.Error(ex);
}
finally
{
	LogManager.Shutdown();
}
