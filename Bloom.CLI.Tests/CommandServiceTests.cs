using System.CommandLine;
using Bloom.CLI.Commands;
using Bloom.CLI.Tests.Substitutes;
using FluentAssertions;

namespace Bloom.CLI.Tests;

public sealed class CommandServiceTests {
    [Fact]
    public async Task Process_WithNoCommandModules_WithoutArguments_ExitsWithSuccess() {
        // Arrange
        var console = ConsoleSubstitute.Create(out var getOutputReader, out var getErrorReader);
        var commandService = new CommandService(console, []);

        // Act
        var exitCode = await commandService.Process([]);
        var output = await getOutputReader.Invoke().ReadToEndAsync();
        var error = await getErrorReader.Invoke().ReadToEndAsync();

        // Assert
        exitCode.Should().Be(0);
        output.Should().BeEmpty();
        error.Should().BeEmpty();
    }

    [Fact]
    public async Task Process_WithCommandModules_WithoutArguments_ReportsError() {
        // Arrange
        var console = ConsoleSubstitute.Create(out var getOutputReader, out var getErrorReader);
        var commandModule = new TestCommandModule();
        var commandService = new CommandService(console, [commandModule]);

        // Act
        var exitCode = await commandService.Process([]);
        var output = await getOutputReader.Invoke().ReadToEndAsync();
        var error = await getErrorReader.Invoke().ReadToEndAsync();

        // Assert
        exitCode.Should().Be(1);
        output.Should().NotBeEmpty("the help doc for the command should be present");
        error.Should().StartWith("Required command was not provided.");
        commandModule.IsHandled.Should().BeFalse();
    }

    [Fact]
    public async Task Process_WithCommandModules_WithCommandArguments_HandlesCommand() {
        // Arrange
        var console = ConsoleSubstitute.Create(out var getOutputReader, out var getErrorReader);
        var commandModule = new TestCommandModule();
        var commandService = new CommandService(console, [commandModule]);

        // Act
        var exitCode = await commandService.Process(["test"]);
        var output = await getOutputReader.Invoke().ReadToEndAsync();
        var error = await getErrorReader.Invoke().ReadToEndAsync();

        // Assert
        exitCode.Should().Be(0);
        output.Should().BeEmpty();
        error.Should().BeEmpty();
        commandModule.IsHandled.Should().BeTrue();
    }

    private sealed class TestCommandModule : ICommandModule {
        public bool IsHandled { get; private set; }

        public Command Build() {
            var command = new Command("test");

            command.SetHandler(() => IsHandled = true);

            return command;
        }
    }
}