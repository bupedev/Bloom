using System.CommandLine;
using System.CommandLine.IO;
using Bloom.CLI;
using Bloom.CLI.Commands;
using Bloom.CLI.FileSystem;
using Bloom.Language;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Dependency injection configuration
using var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices(services => {
            // Register the system library abstractions
            services.AddScoped<IConsole, SystemConsole>();
            services.AddScoped<IFileSystem, StandardFileSystem>();

            // Register the core command service (entry point for the program) and command modules
            services.AddScoped<CommandService>();
            services.AddScoped<ICommandModule, LexerCommandModule>();

            // Register language services
            services.AddScoped<ILexer, Lexer>();
        }
    )
    .Build();

// Resolve the command service and process the arguments
return await host
    .Services
    .GetRequiredService<CommandService>()
    .Process(args);