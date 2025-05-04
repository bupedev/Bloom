using System.CommandLine;
using System.CommandLine.IO;
using System.Text;
using NSubstitute;

namespace Bloom.CLI.Tests.Substitutes;

internal static class ConsoleSubstitute {
    internal static IConsole Create(out Func<StringReader> getOutputReader, out Func<StringReader> getErrorReader) {
        var outputBuilder = new StringBuilder();
        var errorBuilder = new StringBuilder();

        // Substitute the stream writers that IConsole exposes
        var outputStreamWriter = Substitute.For<IStandardStreamWriter>();
        outputStreamWriter
            .When(x => x.Write(Arg.Any<string>()))
            .Do(ci => outputBuilder.Append(ci.Arg<string>()));

        var errorStreamWriter = Substitute.For<IStandardStreamWriter>();
        errorStreamWriter
            .When(x => x.Write(Arg.Any<string>()))
            .Do(ci => errorBuilder.Append(ci.Arg<string>()));

        // Substitute IConsole itself
        var console = Substitute.For<IConsole>();
        console.Out.Returns(outputStreamWriter);
        console.Error.Returns(errorStreamWriter);

        // Prepare partial readers for output and error streams
        getOutputReader = () => {
            var reader = new StringReader(outputBuilder.ToString());
            outputBuilder.Clear();
            return reader;
        };
        getErrorReader = () => {
            var reader = new StringReader(errorBuilder.ToString());
            errorBuilder.Clear();
            return reader;
        };
        return console;
    }
}