using FluentAssertions;

namespace Bloom.CLI.Tests.Substitutes.Tests;

public sealed class FileSystemSubstituteTests {
    [Fact]
    public void Create_WithNoFiles_ReturnsFileSystemSubstitute() {
        // Act
        var fileSystem = FileSystemSubstitute.Create();

        // Assert
        fileSystem.Should().NotBeNull();
        fileSystem.FileExists("any.file").Should().BeFalse();
    }

    [Fact]
    public void Create_WithSingleFile_ReturnsFileSystemSubstitute() {
        // Act
        var fileSystem = FileSystemSubstitute.Create(("test.txt", "contents"));

        // Assert
        fileSystem.Should().NotBeNull();
        fileSystem.FileExists("test.txt").Should().BeTrue();
        fileSystem.FileReadAllText("test.txt").Should().Be("contents");
    }

    [Fact]
    public void Create_WithMultipleFiles_ReturnsFileSystemSubstitute() {
        // Act
        var fileSystem = FileSystemSubstitute.Create(
            ("test1.txt", "contents1"),
            ("test2.txt", "contents2")
        );

        // Assert
        fileSystem.Should().NotBeNull();
        fileSystem.FileExists("test1.txt").Should().BeTrue();
        fileSystem.FileExists("test2.txt").Should().BeTrue();
        fileSystem.FileReadAllText("test1.txt").Should().Be("contents1");
        fileSystem.FileReadAllText("test2.txt").Should().Be("contents2");
    }

    [Fact]
    public void FileExists_WithNonExistentFile_ReturnsFalse() {
        // Arrange
        var fileSystem = FileSystemSubstitute.Create(("test.txt", "contents"));

        // Act & Assert
        fileSystem.FileExists("nonexistent.txt").Should().BeFalse();
    }

    [Fact]
    public void FileReadAllText_WithNonExistentFile_Throws() {
        // Arrange
        var fileSystem = FileSystemSubstitute.Create(("test.txt", "contents"));

        // Act
        var action = () => fileSystem.FileReadAllText("nonexistent.txt");

        // Assert
        action.Should().Throw<KeyNotFoundException>();
    }
}
