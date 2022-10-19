using V22WebServer.Controllers;
using V22WebServer.MiddleWare;
using V22WebServer.Service;
using V22WebServer.Service.Database;
using V22WebServer.Service.Redis;
using ZLogger;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.SetBasePath(Directory.GetCurrentDirectory());
        config.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);
    });

builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConfiguration(builder.Configuration.GetSection("logging"));
    logging.AddZLoggerConsole();
    logging.AddZLoggerFile("ptLog.log");
    logging.AddZLoggerRollingFile((dt,x) => $"logs/{dt.ToLocalTime():yyyy-MM-dd}_{x:000}.log", x => x.ToLocalTime().Date, 1024);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UserSessionMiddleWare();

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
Database.Init(builder.Configuration.GetSection("DBConnection")["MySqlGame"]);
Redis.Init(builder.Configuration.GetSection("DBConnection")["Redis"]);
app.Run();