using System.CommandLine;

var rootCommand = new RootCommand("Bloom Command Line Interface");

// Lexing (scanning) command configuration.
var lexCommand = new Command("lex", "Lex (scan) Bloomish code.");

var fileOption = new Option<FileInfo>(
    ["-f", "--file"],
    "A file containing Bloomish code to be lexed."
).ExistingOnly();

lexCommand.AddOption(fileOption);

var codeOption = new Option<string>(
    ["-c", "--code"],
    "In-line Bloomish code to be lexed. If provided, the \"file\" argument will be ignored."
);
lexCommand.AddOption(codeOption);

lexCommand.AddValidator(result => {
        var hasFile = result.GetValueForOption(fileOption) is not null;
        var hasCode = result.GetValueForOption(codeOption) is not null;
        if (hasFile == hasCode) result.ErrorMessage = "Specify exactly one of '--file' or '--code'.";
    }
);

lexCommand.SetHandler(
    (code, file) => {
        var source = !string.IsNullOrEmpty(code) ? code : File.ReadAllText(file.FullName);

        Console.WriteLine($"Provided code: {source}");
        // TODO: Add seam to lexing code...
    },
    codeOption,
    fileOption
);

rootCommand.Add(lexCommand);

rootCommand.SetHandler(() => {
        Console.WriteLine("Welcome to the Bloom Command Line Interface! Functionality coming soon!");
    }
);

return await rootCommand.InvokeAsync(args);