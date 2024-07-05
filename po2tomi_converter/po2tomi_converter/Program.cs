using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using po2tomi_converter.Commands;
using po2tomi_converter.Settings;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var serviceProvider = new ServiceCollection()
    .Configure<MainSettings>(configuration.GetSection(nameof(MainSettings)))
    .AddTransient<ToPoCommand>()
    .BuildServiceProvider();

var toPoCommand = serviceProvider.GetService<ToPoCommand>();

toPoCommand.Execute();