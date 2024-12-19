using TagsCloudCreation.Configs;

namespace TagsCloudCreation.TagsDrawingDecorators;

public class SingleFontTagsDecorator : ITagsDrawingDecorator
{
    private readonly ITagsFontConfig _fontConfig;

    public SingleFontTagsDecorator(ITagsFontConfig fontConfig)
    {
        ArgumentNullException.ThrowIfNull(fontConfig);

        _fontConfig = fontConfig;
    }

    public TagDrawing[] Decorate(IList<TagDrawing> tags)
    {
        ArgumentNullException.ThrowIfNull(tags);

        return tags
            .Select(tag => tag with
            {
                FontName = _fontConfig.FontName,
                FontStyle = _fontConfig.FontStyle,
            })
            .ToArray();
    }
}
