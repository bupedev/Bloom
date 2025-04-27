using System.CommandLine;

var rootCommand = new RootCommand("Bloom Command Line Interface");

rootCommand.SetHandler(() =>
{
    Console.WriteLine("Welcome to the Bloom Command Line Interface! Functionality coming soon!");
});

return await rootCommand.InvokeAsync(args);