using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using po2tomi_converter.Commands;
using po2tomi_converter.Settings;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var serviceProvider = new ServiceCollection()
    .Configure<MainSettings>(configuration.GetSection(nameof(MainSettings)))
    .AddTransient<ToPoCommand>()
    .AddTransient<FromPoCommand>()
    .BuildServiceProvider();

var options = serviceProvider.GetService<IOptions<MainSettings>>();
if (options.Value.ToPoConversion)
{
    var toPoCommand = serviceProvider.GetService<ToPoCommand>();
    toPoCommand.Execute();
}
else
{
    var fromPoCommand = serviceProvider.GetService<FromPoCommand>();
    fromPoCommand.Execute();
}
