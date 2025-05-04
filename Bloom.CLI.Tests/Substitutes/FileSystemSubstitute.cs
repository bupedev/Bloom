using System.Collections.Immutable;
using Bloom.CLI.FileSystem;
using NSubstitute;

namespace Bloom.CLI.Tests.Substitutes;

internal static class FileSystemSubstitute {
    internal static IFileSystem Create(params (string Name, string Contents)[] files) {
        var filesMap = files.ToImmutableDictionary(x => x.Name, x => x.Contents);

        var fileSystem = Substitute.For<IFileSystem>();
        fileSystem.FileExists(Arg.Any<string>()).Returns(x => filesMap.ContainsKey(x.Arg<string>()));
        fileSystem.FileReadAllText(Arg.Any<string>()).Returns(x => filesMap[x.Arg<string>()]);

        return fileSystem;
    }
}