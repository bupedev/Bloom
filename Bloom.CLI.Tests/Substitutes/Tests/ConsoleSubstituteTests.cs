using System.CommandLine;
using FluentAssertions;

namespace Bloom.CLI.Tests.Substitutes.Tests;

public sealed class ConsoleSubstituteTests {
    [Fact]
    public void Create_ShouldReturnConsoleAndProvideOutputStreams() {
        // Act
        var console = ConsoleSubstitute.Create(out var getOutputReader, out var getErrorReader);

        // Assert
        console.Should().NotBeNull();
        getOutputReader.Should().NotBeNull();
        getErrorReader.Should().NotBeNull();
    }

    [Fact]
    public void Write_ShouldWriteToStandardOutput() {
        // Arrange
        var console = ConsoleSubstitute.Create(out var getOutputReader, out _);
        var testMessage = "Test message";

        // Act
        console.Write(testMessage);
        var output = getOutputReader().ReadToEnd();

        // Assert
        output.Should().Be(testMessage);
    }

    [Fact]
    public void GetOutputReader_ShouldClearPreviouslyReadTextAfterReading() {
        // Arrange
        var console = ConsoleSubstitute.Create(out var getOutputReader, out _);
        var testMessage1 = "Test message 1";
        var testMessage2 = "Test message 2";

        // Act
        console.Write(testMessage1);
        var output1 = getOutputReader().ReadToEnd();
        console.Write(testMessage2);
        var output2 = getOutputReader().ReadToEnd();

        // Assert
        output1.Should().Be(testMessage1);
        output2.Should().Be(testMessage2);
    }

    [Fact]
    public void WriteError_ShouldWriteToStandardError() {
        // Arrange
        var console = ConsoleSubstitute.Create(out _, out var getErrorReader);
        var testMessage = "Test message";

        // Act
        console.Error.Write(testMessage);
        var output = getErrorReader().ReadToEnd();

        // Assert
        output.Should().Be(testMessage);
    }

    [Fact]
    public void GetErrorReader_ShouldClearPreviouslyReadTextAfterReading() {
        // Arrange
        var console = ConsoleSubstitute.Create(out _, out var getErrorReader);
        var testMessage1 = "Test message 1";
        var testMessage2 = "Test message 2";

        // Act
        console.Error.Write(testMessage1);
        var output1 = getErrorReader().ReadToEnd();
        console.Error.Write(testMessage2);
        var output2 = getErrorReader().ReadToEnd();

        // Assert
        output1.Should().Be(testMessage1);
        output2.Should().Be(testMessage2);
    }
}