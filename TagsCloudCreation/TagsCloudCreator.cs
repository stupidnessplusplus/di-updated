using System.Drawing;
using TagsCloudCreation.WordSizesGetters;
using TagsCloudCreation.TagsDrawingDecorators;
using TagsCloudCreation.TagsDrawers;
using RectanglesCloudPositioning;

namespace TagsCloudCreation;

public class TagsCloudCreator
{
    private readonly IWordSizesGetter _wordSizesGetter;
    private readonly ICircularCloudLayouter _cloudLayouter;
    private readonly IEnumerable<ITagsDrawingDecorator> _tagsSettingsSetters;
    private readonly ITagsDrawer _tagsDrawer;

    public TagsCloudCreator(
        IWordSizesGetter wordSizesGetter,
        ICircularCloudLayouter cloudLayouter,
        IEnumerable<ITagsDrawingDecorator> tagsSettingsSetters,
        ITagsDrawer tagsDrawer)
    {
        _wordSizesGetter = wordSizesGetter;
        _cloudLayouter = cloudLayouter;
        _tagsSettingsSetters = tagsSettingsSetters;
        _tagsDrawer = tagsDrawer;
    }

    public Image DrawTagsCloud(IList<string> words)
    {
        var tags = _wordSizesGetter
            .GetSizes(words)
            .Select(unplacedTag => new Tag(unplacedTag.Word, _cloudLayouter.PutNextRectangle(unplacedTag.Size)))
            .ToArray();

        var tagsWithSettings = GetTagsWithSettings(tags);
        return _tagsDrawer.Draw(tagsWithSettings);
    }

    private TagDrawing[] GetTagsWithSettings(IList<Tag> tags)
    {
        var tagsWithSettings = tags
            .Select(tag => new TagDrawing(tag, default!, default!, default))
            .ToArray();

        return _tagsSettingsSetters
            .Aggregate(tagsWithSettings, (tags, setter) => setter.Decorate(tags));
    }
}
