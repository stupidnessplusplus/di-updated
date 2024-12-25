using CommandLine;
using CommandLine.Text;
using TagsCloudApp;

namespace TagsCloudConsoleInterface;

public static class Program
{
    public static void Main(params string[] args)
    {
        var parser = new Parser(with =>
        {
            with.CaseInsensitiveEnumValues = true;
            with.HelpWriter = null;
        });

        var parserResult = parser.ParseArguments<ProgramConfig>(args);
        parserResult
            .WithParsed(Run)
            .WithNotParsed(_ => DisplayHelp(parserResult));
    }

    private static void Run(ProgramConfig config)
    {
        try
        {
            new App().Run(config, config, config, config, config, config, config);
            Console.WriteLine($"Image saved to '{config.OutputPath}'.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public static void DisplayHelp<T>(ParserResult<T> result)
    {
        var helpText = HelpText.AutoBuild(
            result,
            argHelpText =>
            {
                argHelpText.AddDashesToOption = true;
                argHelpText.AddEnumValuesToHelpText = true;
                return argHelpText;
            });

        Console.WriteLine(helpText);
    }
}
