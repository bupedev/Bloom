using Bloom.CLI;
using Bloom.CLI.Commands;
using Bloom.CLI.Output;
using Bloom.Language;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Dependency injection configuration
using var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices(services => {
            // Register the core command service (entry point for the program)
            services.AddScoped<CommandService>();

            // Register all command modules automatically
            foreach (var commandModuleType in ReflectionUtils.GetImplementingTypes<ICommandModule>())
                services.AddScoped(typeof(ICommandModule), commandModuleType);

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