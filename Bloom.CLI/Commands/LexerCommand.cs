using System.CommandLine;
using Bloom.CLI.FIleSystem;
using Bloom.CLI.Output;
using Bloom.Language;

namespace Bloom.CLI.Commands;

/// <summary>
/// Module for a command that will lex Bloomish code into a series of tokens.
/// </summary>
internal sealed class LexerCommand(IOutputWriter output, IFileSystem fileSystem, ILexer lexer) : ICommandModule {
    /// <inheritdoc />
    public Command Build() {
        var command = new Command("lex", "Lex (scan) Bloomish code to generate an equivalent series of tokens");

        // Allow option for code to be read from a file
        var fileOption = new Option<string>(
            ["-f", "--file"],
            "The path of a file containing Bloomish code to be lexed (mutually exclusive with `--code`)"
        );
        command.AddOption(fileOption);

        // Allow option for code to be read from the command line
        var codeOption = new Option<string>(
            ["-c", "--code"],
            "In-line Bloomish code to be lexed (mutually exclusive with `--file`)"
        );
        command.AddOption(codeOption);

        // Validate that --file' and '--code' options are mutually exclusive
        command.AddValidator(result => {
                var hasFile = result.GetValueForOption(fileOption) is not null;
                var hasCode = result.GetValueForOption(codeOption) is not null;

                if (hasFile == hasCode) result.ErrorMessage = "Specify exactly one of '--file' or '--code'";
            }
        );

        //  Validate that if '--file' is in use, the provided filepath is valid.
        command.AddValidator(result => {
                if (result.GetValueForOption(fileOption) is not { } path) return;
                if (fileSystem.FileExists(path)) result.ErrorMessage = $"No file exists at the path: {path}";
            }
        );

        // Handle the command by calling into the language library and formatting the output
        command.SetHandler(
            (inlineCode, filePath) => {
                var source = !string.IsNullOrEmpty(inlineCode) ? inlineCode : fileSystem.FileReadAllText(filePath);

                foreach (var token in lexer.GenerateTokens(source)) output.WriteLine(token.ToString());
            },
            codeOption,
            fileOption
        );

        return command;
    }
}