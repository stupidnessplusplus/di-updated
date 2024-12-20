using CommandLine;
using System.Drawing;
using TagsCloudApp;
using TagsCloudApp.Configs;
using TagsCloudCreation.Configs;

namespace TagsCloudConsoleInterface;

internal class ProgramConfig : IIOConfig, IDrawingAlgorithmsConfig, IWordSizesGetterConfig, ITagsColorConfig, ITagsFontConfig
{
    [Option('i', "in", Required = true)]
    public string InputPath { get; private set; } = null!;

    [Option('o', "out", Required = true)]
    public string OutputPath { get; private set; } = null!;

    [Option("image-format", Required = false, Default = OutputImageFormat.Png)]
    public OutputImageFormat OutputImageFormat { get; private set; }

    [Option('s', "sizing", Required = false, Default = WordSizingMethod.Frequency)]
    public WordSizingMethod WordSizingMethod { get; private set; }

    [Option('l', "layouter", Required = false, Default = RectanglesLayouter.Spiral)]
    public RectanglesLayouter RectanglesLayouter { get; private set; }

    [Option('d', "drawing-settings", Required = false, Default = new DrawingSetting[0])]
    public DrawingSetting[] DrawingSettings { get; private set; } = null!;

    [Option("min-word-size", Required = false, Default = 10)]
    public int MinSize { get; private set; }

    [Option("background", Required = false, Default = "#000000")]
    public string? BackgroundColorHex
    {
        set
        {
            BackgroundColor = ColorTranslator.FromHtml(value!);
        }
    }

    [Option("foreground", Required = false, Default = "#FFFFFF")]
    public string? MainColorHex
    {
        set
        {
            MainColor = ColorTranslator.FromHtml(value!);
        }
    }

    public Color MainColor { get; private set; }

    public Color BackgroundColor { get; private set; }

    [Option("font", Required = false, Default = "Arial")]
    public string FontName { get; private set; } = null!;

    [Option("font-style", Required = false, Default = FontStyle.Regular)]
    public FontStyle FontStyle { get; private set; }
}
