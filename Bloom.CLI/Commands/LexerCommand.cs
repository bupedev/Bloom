using System.CommandLine;
using Bloom.Language;

namespace Bloom.CLI.Commands;

/// <summary>
/// Module for a command that will lex Bloomish code into a series of tokens.
/// </summary>
internal sealed class LexerCommand(ILexer lexer) : ICommandModule {
    /// <inheritdoc />
    public Command Build() {
        var command = new Command("lex", "Lex (scan) Bloomish code to generate an equivalent series of tokens");

        // Allow option for code to be read from a file
        var fileOption = new Option<FileInfo>(
            ["-f", "--file"],
            "The path of a file containing Bloomish code to be lexed (mutually exclusive with `--code`)"
        ).ExistingOnly();
        command.AddOption(fileOption);

        // Allow option for code to be read from the command line
        var codeOption = new Option<string>(
            ["-c", "--code"],
            "In-line Bloomish code to be lexed (mutually exclusive with `--file`)"
        );
        command.AddOption(codeOption);

        // Validate that '--file' and '--code' options are mutually exclusive
        command.AddValidator(result => {
                var hasFile = result.GetValueForOption(fileOption) is not null;
                var hasCode = result.GetValueForOption(codeOption) is not null;
                if (hasFile == hasCode) result.ErrorMessage = "Specify exactly one of '--file' or '--code'";
            }
        );

        // Handle the command by calling into the language library and formatting the output
        command.SetHandler(
            (code, file) => {
                var source = !string.IsNullOrEmpty(code) ? code : File.ReadAllText(file.FullName);

                foreach (var token in lexer.GenerateTokens(source)) Console.WriteLine($"Provided code: {source}");
            },
            codeOption,
            fileOption
        );

        return command;
    }
}