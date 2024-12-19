using System.Drawing;
using TagsCloudCreation.Configs;

namespace TagsCloudCreation.TagsDrawingDecorators;

public class SingleSolidColorTagsDecorator : ITagsDrawingDecorator
{
    private readonly ITagsColorConfig _colorConfig;

    public SingleSolidColorTagsDecorator(ITagsColorConfig colorConfig)
    {
        ArgumentNullException.ThrowIfNull(colorConfig);

        _colorConfig = colorConfig;
    }

    public TagDrawing[] Decorate(IList<TagDrawing> tags)
    {
        ArgumentNullException.ThrowIfNull(tags);

        var brush = new SolidBrush(_colorConfig.MainColor);
        return tags
            .Select(tag => tag with { Brush = brush })
            .ToArray();
    }
}
