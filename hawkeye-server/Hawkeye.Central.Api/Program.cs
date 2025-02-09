using Hawkeye.Central.Api.MQTT;
using Hawkeye.Central.Api.Options;
using Hawkeye.Central.Api.SignalR;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;

services.AddLogging(opts => opts.AddSimpleConsole());

services.AddControllers();

services.AddSignalR();

var mqttOptions = new MqttOptions();
configuration.GetSection(nameof(MqttOptions)).Bind(mqttOptions);
services.AddSingleton(mqttOptions);
services.AddSingleton<HawkeyeMqttService>();
services.AddHostedService<HawkeyeMqttBackgroundService>();

services.AddAuthentication();
services.AddAuthorization();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapHub<HawkeyeHub>("v1/hub");

app.Run();
