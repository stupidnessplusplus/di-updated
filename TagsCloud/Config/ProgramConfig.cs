using CommandLine;
using RectanglesCloudPositioning;
using System.Drawing;
using System.Drawing.Imaging;
using TagsCloudCreation.Configs;
using TagsCloudCreation.WordSizesGetters;

namespace TagsCloud.Config;

internal class ProgramConfig : IWordSizesGetterConfig, ITagsColorConfig, ITagsFontConfig
{
    #region IO

    [Option('i', "in", Required = true)]
    public string InputPath { get; private set; } = null!;

    [Option('o', "out", Required = true)]
    public string OutputPath { get; private set; } = null!;

    [Option("image-format", Required = false, Default = OutputImageFormat.Png)]
    public OutputImageFormat OutputImageFormat
    {
        set
        {
            ImageFormat = value switch
            {
                OutputImageFormat.Png => ImageFormat.Png,
                OutputImageFormat.Jpeg => ImageFormat.Jpeg,
                OutputImageFormat.Bmp => ImageFormat.Bmp,
                _ => throw new ArgumentException("Unsupported image format.")
            };
        }
    }

    public ImageFormat ImageFormat { get; private set; } = null!;

    #endregion

    #region Drawing algorithms

    private static readonly Dictionary<WordSizingMethod, Type> _sizingMethodTypes = new()
    {
        { WordSizingMethod.Frequency, typeof(WordSizesByFrequencyGetter) },
    };

    private static readonly Dictionary<RectanglesLayouter, Type> _rectanglesLayoutersTypes = new()
    {
        { RectanglesLayouter.Spiral, typeof(SpiralCloudLayouter) },
    };

    private static readonly Dictionary<DrawingSetting, Type> _drawingSettingsSetterTypes = new()
    {
    };

    [Option('s', "sizing", Required = false, Default = WordSizingMethod.Frequency)]
    public WordSizingMethod WordSizingMethod
    {
        set
        {
            ArgumentNullException.ThrowIfNull(value);

            if (!_sizingMethodTypes.TryGetValue(value, out var type))
            {
                throw new ArgumentException($"Unknown word sizing method type: '{value}'.");
            }

            WordSizesGetterType = type;
        }
    }

    [Option('l', "layouter", Required = false, Default = RectanglesLayouter.Spiral)]
    public RectanglesLayouter RectanglesLayouter

    {
        set
        {
            ArgumentNullException.ThrowIfNull(value);

            if (!_rectanglesLayoutersTypes.TryGetValue(value, out var type))
            {
                throw new ArgumentException($"Unknown rectangle layouter type: '{value}'.");
            }

            RectanglesLayouterType = type;
        }
    }

    [Option('d', "drawing-settings", Required = false, Default = new DrawingSetting[0])]
    public DrawingSetting[] DrawingSettings
    {
        set
        {
            ArgumentNullException.ThrowIfNull(value);

            var result = new Type[value.Length];

            for (var i = 0; i < value.Length; i++)
            {
                if (!_drawingSettingsSetterTypes.TryGetValue(value[i], out var type))
                {
                    throw new ArgumentException($"Unknown setting: '{value[i]}'.");
                }

                result[i] = type;
            }

            AdditionalSettingsSetterTypes = result;
        }
    }

    public Type WordSizesGetterType { get; private set; } = null!;

    public Type RectanglesLayouterType { get; private set; } = null!;

    public Type[] AdditionalSettingsSetterTypes { get; private set; } = null!;

    #endregion

    #region IWordSizesGetterConfig

    [Option("min-word-size", Required = false, Default = 10)]
    public int MinSize { get; private set; }

    #endregion

    #region ITagsColorConfig

    [Option("background", Required = false, Default = "#000000")]
    public string? BackgroundColorHex
    {
        set
        {
            BackgroundColor = ColorTranslator.FromHtml(value!);
        }
    }

    [Option("foreground", Required = false, Default = "#FFFFFF")]
    public string? WordsColorHex
    {
        set
        {
            MainColor = ColorTranslator.FromHtml(value!);
        }
    }

    public Color MainColor { get; private set; }

    public Color BackgroundColor { get; private set; }

    #endregion

    #region ITagsFontConfig

    [Option("font", Required = false, Default = "Arial")]
    public string FontName { get; private set; } = null!;

    [Option("font-style", Required = false, Default = FontStyle.Regular)]
    public FontStyle FontStyle { get; private set; }

    #endregion
}
