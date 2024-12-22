﻿using RectanglesCloudPositioning;
using TagsCloudCreation.WordSizesGetters;

namespace TagsCloudApp.Configs;

public interface IDrawingAlgorithmsConfig
{
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

    public WordSizingMethod WordSizingMethod { get; }

    public RectanglesLayouter RectanglesLayouter { get; }

    public DrawingSetting[] DrawingSettings { get; }

    internal Type WordSizesGetterType
    {
        get
        {
            if (!_sizingMethodTypes.TryGetValue(WordSizingMethod, out var type))
            {
                throw new ArgumentException($"Unknown word sizing method type: '{WordSizingMethod}'.");
            }

            return type;
        }
    }

    internal Type RectanglesLayouterType
    {
        get
        {
            if (!_rectanglesLayoutersTypes.TryGetValue(RectanglesLayouter, out var type))
            {
                throw new ArgumentException($"Unknown rectangle layouter type: '{RectanglesLayouter}'.");
            }

            return type;
        }
    }

    internal Type[] AdditionalSettingsSetterTypes
    {
        get
        {
            var result = new Type[DrawingSettings.Length];

            for (var i = 0; i < DrawingSettings.Length; i++)
            {
                if (!_drawingSettingsSetterTypes.TryGetValue(DrawingSettings[i], out var type))
                {
                    throw new ArgumentException($"Unknown setting: '{DrawingSettings[i]}'.");
                }

                result[i] = type;
            }

            return result;
        }
    }
}