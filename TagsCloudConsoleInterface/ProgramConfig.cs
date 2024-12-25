using CommandLine;
using RectanglesCloudPositioning.Configs;
using System.Drawing;
using TagsCloudApp;
using TagsCloudApp.Configs;
using TagsCloudCreation.Configs;
using WordsFiltration;
using WordsFiltration.Configs;

namespace TagsCloudConsoleInterface;

internal class ProgramConfig
    : IIOConfig,
      IDrawingAlgorithmsConfig,
      IWordsSelectionConfig,
      IWordSizesGetterConfig,
      IRectanglesPositioningConfig,
      ITagsColorConfig,
      ITagsFontConfig
{
    [Option('i', "in", Required = true)]
    public string InputPath { get; private set; } = null!;

    [Option('o', "out", Required = true)]
    public string OutputPath { get; private set; } = null!;

    [Option("image-format", Required = false, Default = OutputImageFormat.Png)]
    public OutputImageFormat OutputImageFormat { get; private set; }

    [Option('s', "sizing", Required = false, Default = WordSizingMethod.Frequency)]
    public WordSizingMethod WordSizingMethod { get; private set; }

    [Option('l', "layouter", Required = false, Default = RectanglesLayouter.Circle)]
    public RectanglesLayouter RectanglesLayouter { get; private set; }

    [Option('d', "drawing-settings", Required = false, Default = new DrawingSetting[0])]
    public IEnumerable<DrawingSetting> DrawingSettingsEnumerable
    {
        set
        {
            DrawingSettings = value.ToArray();
        }
    }

    public DrawingSetting[] DrawingSettings { get; private set; } = null!;

    [Option("exclude-words", Required = false, Default = null)]
    public string? ExcludedWordsPath
    {
        set
        {
            if (value == null)
            {
                return;
            }

            ExcludedWords = File.ReadAllLines(value);
        }
    }

    public string[]? ExcludedWords { get; private set; }

    [Option("pos", Required = false, Default = new PartOfSpeech[] { PartOfSpeech.A, PartOfSpeech.S, PartOfSpeech.V })]
    public IEnumerable<PartOfSpeech> IncludedPartsOfSpeechEnumerable
    {
        set
        {
            IncludedPartsOfSpeech = value.ToArray();
        }
    }

    public PartOfSpeech[] IncludedPartsOfSpeech { get; private set; } = null!;

    [Option("min-word-size", Required = false, Default = 10)]
    public int MinSize { get; private set; }

    [Option("word-size-scale", Required = false, Default = 1)]
    public double Scale { get; private set; }

    public Point Center => Point.Empty;

    [Option("rays-count", Required = false, Default = 360)]
    public int RaysCount { get; private set; }

    [Option("radius", Required = false, Default = "1")]
    public string RadiusEquationString
    {
        set
        {
            try
            {
                RadiusEquation = RadiusEquationParser.ParseRadiusEquation(value);
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"'{value}' is not a radius equation.", ex);
            }
        }
    }

    public Func<double, double> RadiusEquation { get; private set; } = null!;

    [Option("background", Required = false, Default = "#000000")]
    public string BackgroundColorHex
    {
        set
        {
            BackgroundColor = ColorTranslator.FromHtml(value);
        }
    }

    [Option("main-color", Required = false, Default = "#FFFFFF")]
    public string MainColorHex
    {
        set
        {
            MainColor = ColorTranslator.FromHtml(value);
        }
    }

    public Color MainColor { get; private set; }

    [Option("secondary-color", Required = false, Default = "#FFFFFF")]
    public string SecondaryColorHex
    {
        set
        {
            SecondaryColor = ColorTranslator.FromHtml(value);
        }
    }

    public Color SecondaryColor { get; private set; }

    public Color BackgroundColor { get; private set; }

    [Option("font", Required = false, Default = "Arial")]
    public string FontName { get; private set; } = null!;

    [Option("font-style", Required = false, Default = FontStyle.Regular)]
    public FontStyle FontStyle { get; private set; }
}
