using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Settings.Configuration;

// Using multiple logging configurations ###########################################################
using var logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/primary.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

using var altLogger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .WriteTo.File("logs/alternate.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

logger.Information("Information message with {param}", "some-param-value");
altLogger.Information("Information message with {param}", "some-alt-param-value");


// Using a JSON file configuration ################################################################
IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("config.json") //, optional: true, reloadOnChange: true)
    .Build();


var minimumLevel = configuration.GetSection("Serilog").GetValue<string>("MinimumLevel");
logger.Information("MinimumLevel from config is {0}", minimumLevel);

using var configLogger = new LoggerConfiguration()
    .ReadFrom
    .Configuration(configuration)
    .CreateLogger();

configLogger.Warning("Warning message with {param}", "some-config-param-value");

// The alternate configuration has a different minimum level
using var altConfigLogger = new LoggerConfiguration()
    .ReadFrom
    .Configuration(configuration, new ConfigurationReaderOptions { SectionName = "AltSerilog" })
    .CreateLogger();
    
altConfigLogger.Verbose("Verbose message to alt config logger");
    