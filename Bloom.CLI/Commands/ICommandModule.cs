using System.CommandLine;

namespace Bloom.CLI.Commands;

/// <summary>
/// Contract for command modules that are integrated into the CLI application. Each implementation is responsible for
/// configuring its command.
/// </summary>
internal interface ICommandModule {
    /// <summary>
    /// Creates and configures the <see cref="Command" /> for the module.
    /// </summary>
    public Command Build();
}