namespace Bloom.CLI.FIleSystem;

/// <summary>
/// Abstraction for file system operations.
/// </summary>
internal interface IFileSystem {
    /// <summary>
    /// Checks if a file exists at the specified file path.
    /// </summary>
    /// <param name="filePath"> The path of the file to check. </param>
    /// <returns> True if the file exists; false otherwise. </returns>
    bool FileExists(string filePath);

    /// <summary>
    /// Reads the content of a file at the specified file path.
    /// </summary>
    /// <param name="filePath"> The path of the file to read. </param>
    /// <returns> The content of the file as a string. </returns>
    string FileReadAllText(string filePath);
}