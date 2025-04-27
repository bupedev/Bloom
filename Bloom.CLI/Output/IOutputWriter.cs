namespace Bloom.CLI.Output;

/// <summary>
/// Abstraction for output writing operations.
/// </summary>
internal interface IOutputWriter {
    /// <summary>
    /// Writes a string followed by a line terminator to the output.
    /// </summary>
    /// <param name="text"> The text to be written as a single line. </param>
    public void WriteLine(string text);
}