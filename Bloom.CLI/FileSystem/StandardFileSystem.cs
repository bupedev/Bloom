namespace Bloom.CLI.FileSystem;

/// <summary>
/// Handles file system operations via the system library.
/// </summary>
internal sealed class StandardFileSystem : IFileSystem {
    /// <inheritdoc />
    public bool FileExists(string filePath) {
        return File.Exists(filePath);
    }

    /// <inheritdoc />
    public string FileReadAllText(string filePath) {
        return File.ReadAllText(filePath);
    }
}