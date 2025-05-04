using System.CommandLine;
using Bloom.CLI.Commands;
using Bloom.CLI.Tests.Substitutes;
using Bloom.Language;
using FluentAssertions;
using NSubstitute;

namespace Bloom.CLI.Tests.Commands;

public sealed class LexerCommandModuleTests {
    [Fact]
    public void Build_ShouldNotThrow() {
        // Arrange
        var console = ConsoleSubstitute.Create(out _, out _);
        var fileSystem = FileSystemSubstitute.Create();
        var lexer = Substitute.For<ILexer>();
        var commandModule = new LexerCommandModule(console, fileSystem, lexer);

        // Act
        Action build = () => _ = commandModule.Build();

        // Assert
        build.Should().NotThrow();
    }

    [Fact]
    public void Process_OnBuiltCommand_WithNoArguments_ReportsError() {
        // Arrange
        var console = ConsoleSubstitute.Create(out var getOutputReader, out var getErrorReader);
        var fileSystem = FileSystemSubstitute.Create();
        var lexer = Substitute.For<ILexer>();
        var command = new LexerCommandModule(console, fileSystem, lexer).Build();

        // Act
        var exitCode = command.Invoke([], console);
        var output = getOutputReader.Invoke().ReadToEnd();
        var error = getErrorReader.Invoke().ReadToEnd();

        // Assert
        exitCode.Should().Be(1);
        output.Should().NotBeEmpty("the help doc for the command should be present");
        error.Should().StartWith("Specify exactly one of '--file' or '--code'.");
    }

    [Fact]
    public void Process_OnBuiltCommand_WithBothCodeAndFileOptions_ReportsError() {
        // Arrange
        var console = ConsoleSubstitute.Create(out var getOutputReader, out var getErrorReader);
        var fileSystem = FileSystemSubstitute.Create();
        var lexer = Substitute.For<ILexer>();
        var command = new LexerCommandModule(console, fileSystem, lexer).Build();

        // Act
        var exitCode = command.Invoke(["--code", "foo", "--file", "bar.blm"], console);
        var output = getOutputReader.Invoke().ReadToEnd();
        var error = getErrorReader.Invoke().ReadToEnd();

        // Assert
        exitCode.Should().Be(1);
        output.Should().NotBeEmpty("the help doc for the command should be present");
        error.Should().StartWith("Specify exactly one of '--file' or '--code'.");
    }

    [Fact]
    public void Process_OnBuiltCommand_WithCodeOptionSpecified_CallsLexer() {
        // Arrange
        var console = ConsoleSubstitute.Create(out var getOutputReader, out var getErrorReader);
        var fileSystem = FileSystemSubstitute.Create();
        var lexer = Substitute.For<ILexer>();
        lexer
            .GenerateTokens(Arg.Any<IEnumerable<char>>())
            .Returns(x => x.Arg<IEnumerable<char>>().Select(c => new Token(TokenType.Symbol, c.ToString())));
        var command = new LexerCommandModule(console, fileSystem, lexer).Build();

        // Act
        var exitCode = command.Invoke(["--code", "foo"], console);
        var output = getOutputReader.Invoke().ReadToEnd();
        var error = getErrorReader.Invoke().ReadToEnd();

        // Assert
        exitCode.Should().Be(0);
        output
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Should()
            .BeEquivalentTo(
                new Token(TokenType.Symbol, "f").ToString(),
                new Token(TokenType.Symbol, "o").ToString(),
                new Token(TokenType.Symbol, "o").ToString()
            );
        error.Should().BeEmpty();
    }

    [Fact]
    public void Process_OnBuiltCommand_WithFileOptionSpecified_WhenFileIsMissing_ReportsError() {
        // Arrange
        var console = ConsoleSubstitute.Create(out var getOutputReader, out var getErrorReader);
        var fileSystem = FileSystemSubstitute.Create();
        var lexer = Substitute.For<ILexer>();
        lexer
            .GenerateTokens(Arg.Any<IEnumerable<char>>())
            .Returns(x => x.Arg<IEnumerable<char>>().Select(c => new Token(TokenType.Symbol, c.ToString())));
        var command = new LexerCommandModule(console, fileSystem, lexer).Build();

        // Act
        var exitCode = command.Invoke(["--file", "bar.blm"], console);
        var output = getOutputReader.Invoke().ReadToEnd();
        var error = getErrorReader.Invoke().ReadToEnd();

        // Assert
        exitCode.Should().Be(1);
        output.Should().NotBeEmpty("the help doc for the command should be present");
        error.Should().StartWith("No file exists at the path: bar.blm");
    }

    [Fact]
    public void Process_OnBuiltCommand_WithFileOptionSpecified_CallsLexer() {
        // Arrange
        var console = ConsoleSubstitute.Create(out var getOutputReader, out var getErrorReader);
        var fileSystem = FileSystemSubstitute.Create(("bar.blm", "bar"));
        var lexer = Substitute.For<ILexer>();
        lexer
            .GenerateTokens(Arg.Any<IEnumerable<char>>())
            .Returns(x => x.Arg<IEnumerable<char>>().Select(c => new Token(TokenType.Symbol, c.ToString())));
        var command = new LexerCommandModule(console, fileSystem, lexer).Build();

        // Act
        var exitCode = command.Invoke(["--file", "bar.blm"], console);
        var output = getOutputReader.Invoke().ReadToEnd();
        var error = getErrorReader.Invoke().ReadToEnd();

        // Assert
        exitCode.Should().Be(0);
        output
            .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
            .Should()
            .BeEquivalentTo(
                new Token(TokenType.Symbol, "b").ToString(),
                new Token(TokenType.Symbol, "a").ToString(),
                new Token(TokenType.Symbol, "r").ToString()
            );
        error.Should().BeEmpty();
    }
}