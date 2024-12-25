using CommandLine;
using TagsCloudApp;

namespace TagsCloudConsoleInterface;

public static class Program
{
    public static void Main(params string[] args)
    {
        Parser.Default
            .ParseArguments<ProgramConfig>(args)
            .WithParsed(Run);
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
}
