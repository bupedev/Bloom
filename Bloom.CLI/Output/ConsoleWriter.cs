namespace Bloom.CLI.Output;

/// <summary>
/// Handles output writing operations via the console.
/// </summary>
internal sealed class ConsoleWriter : IOutputWriter {
    /// <inheritdoc />
    public void WriteLine(string text) {
        Console.Out.WriteLine(text);
    }
}