using System.CommandLine;
using Bloom.CLI.Commands;

namespace Bloom.CLI;

/// <summary>
/// Core service for orchestrating commands in the command-line interface.
/// </summary>
internal sealed class CommandService {
    private readonly Command _rootCommand;

    public CommandService(IEnumerable<ICommandModule> commandModules) {
        _rootCommand = new RootCommand("All-in-one command-line toolkit for Bloomish language development");

        // Add each of the registered command modules to the root command.
        foreach (var commandModule in commandModules) _rootCommand.AddCommand(commandModule.Build());
    }

    /// <summary>
    /// Processes command-line arguments using the configured root command.
    /// </summary>
    /// <param name="args"> An array of strings representing the command-line arguments. </param>
    /// <returns>
    /// A task that represents the asynchronous processing of the command-line arguments, returning an integer exit
    /// code upon completion.
    /// </returns>
    public Task<int> Process(string[] args) {
        return _rootCommand.InvokeAsync(args);
    }
}