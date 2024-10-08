﻿using Microsoft.Extensions.Configuration;
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
    .AddTransient<SortCommand>()
    .BuildServiceProvider();

var options = serviceProvider.GetService<IOptions<MainSettings>>();

try
{
    switch (options.Value.Mode)
    {
        case Mode.ToPo:
            var toPoCommand = serviceProvider.GetService<ToPoCommand>();
            if (!toPoCommand.HasErrors)
                toPoCommand.Execute();
            break;
        case Mode.FromPo:
            var fromPoCommand = serviceProvider.GetService<FromPoCommand>();
            fromPoCommand.Execute();
            break;
        case Mode.Sort:
            var sortCommand = serviceProvider.GetService<SortCommand>();
            sortCommand.Execute();
            break;
        default:
            break;
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.ToString());
    Console.ReadKey();
}


