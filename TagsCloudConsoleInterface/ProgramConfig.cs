using CommandLine;
using RectanglesCloudPositioning.Configs;
using System.Drawing;
using System.Drawing.Text;
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
    private string inputPath = null!;
    private string outputPath = null!;
    private string excludedWordsPath = null!;
    private string radiusEquationString = null!;
    private int minWordSize;
    private double scale;
    private int raysCount;
    private string fontName = null!;
    private FontStyle fontStyle;

    [Option('i', "in", Required = true)]
    public string InputPath
    {
        get => inputPath;
        set
        {
            if (!File.Exists(value))
            {
                throw new FileNotFoundException($"Input file not found: '{value}'.");
            }

            inputPath = value;
        }
    }

    [Option('o', "out", Required = true)]
    public string OutputPath
    {
        get => outputPath;
        set => outputPath = value;
    }

    [Option("image-format", Required = false, Default = OutputImageFormat.Png)]
    public OutputImageFormat OutputImageFormat { get; private set; }

    [Option('s', "sizing", Required = false, Default = WordSizingMethod.Frequency)]
    public WordSizingMethod WordSizingMethod { get; private set; }

    [Option('l', "layouter", Required = false, Default = RectanglesLayouter.Circle)]
    public RectanglesLayouter RectanglesLayouter { get; private set; }

    [Option('d', "drawing-settings", Required = false, Default = new DrawingSetting[0])]
    public IEnumerable<DrawingSetting> DrawingSettingsEnumerable
    {
        get => DrawingSettings;
        private set
        {
            DrawingSettings = value.ToArray();
        }
    }

    public DrawingSetting[] DrawingSettings { get; private set; } = null!;

    [Option("exclude-words", Required = false, Default = null)]
    public string? ExcludedWordsPath
    {
        get => excludedWordsPath;
        private set
        {
            if (value == null)
            {
                ExcludedWords = null;
                return;
            }

            if (!File.Exists(value))
            {
                throw new FileNotFoundException($"Excluded words file not found: '{value}'.");
            }

            excludedWordsPath = value;
            ExcludedWords = File.ReadAllLines(value);
        }
    }

    public string[]? ExcludedWords { get; private set; }

    [Option("pos", Required = false, Default = new PartOfSpeech[] { PartOfSpeech.A, PartOfSpeech.S, PartOfSpeech.V })]
    public IEnumerable<PartOfSpeech> IncludedPartsOfSpeechEnumerable
    {
        get => IncludedPartsOfSpeech;
        private set
        {
            IncludedPartsOfSpeech = value.ToArray();
        }
    }

    public PartOfSpeech[] IncludedPartsOfSpeech { get; private set; } = null!;

    [Option("min-word-size", Required = false, Default = 10)]
    public int MinSize
    {
        get => minWordSize;
        private set
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);

            minWordSize = value;
        }
    }

    [Option("word-size-scale", Required = false, Default = 1)]
    public double Scale
    {
        get => scale;
        private set
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);

            scale = value;
        }
    }

    public Point Center => Point.Empty;

    [Option("rays-count", Required = false, Default = 360)]
    public int RaysCount
    {
        get => raysCount;
        private set
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(value);

            raysCount = value;
        }
    }

    [Option("radius", Required = false, Default = "1")]
    public string RadiusEquationString
    {
        get => radiusEquationString;
        private set
        {
            try
            {
                radiusEquationString = value;
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
        get => ColorTranslator.ToHtml(BackgroundColor);
        private set => BackgroundColor = ColorTranslator.FromHtml(value);
    }

    [Option("main-color", Required = false, Default = "#FFFFFF")]
    public string MainColorHex
    {
        get => ColorTranslator.ToHtml(MainColor);
        private set => MainColor = ColorTranslator.FromHtml(value);
    }

    [Option("secondary-color", Required = false, Default = "#FFFFFF")]
    public string SecondaryColorHex
    {
        get => ColorTranslator.ToHtml(SecondaryColor);
        private set => SecondaryColor = ColorTranslator.FromHtml(value);
    }

    public Color BackgroundColor { get; private set; }

    public Color MainColor { get; private set; }

    public Color SecondaryColor { get; private set; }

    [Option("font", Required = false, Default = "Arial")]
    public string FontName
    {
        get => fontName;
        set
        {
            using var installedFonts = new InstalledFontCollection();

            if (!installedFonts.Families
                .Any(fontFamily => fontFamily.Name.Equals(value, StringComparison.InvariantCultureIgnoreCase)))
            {
                throw new Exception($"Font '{value}' does not exist.");
            }

            fontName = value;
        }
    }

    [Option("font-style", Required = false, Default = FontStyle.Regular)]
    public FontStyle FontStyle { get; private set; }
}
