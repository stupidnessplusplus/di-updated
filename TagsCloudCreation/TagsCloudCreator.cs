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
        ArgumentNullException.ThrowIfNull(wordSizesGetter);
        ArgumentNullException.ThrowIfNull(cloudLayouter);
        ArgumentNullException.ThrowIfNull(tagsSettingsSetters);
        ArgumentNullException.ThrowIfNull(tagsDrawer);

        _wordSizesGetter = wordSizesGetter;
        _cloudLayouter = cloudLayouter;
        _tagsSettingsSetters = tagsSettingsSetters;
        _tagsDrawer = tagsDrawer;
    }

    public Bitmap DrawTagsCloud(IList<string> words)
    {
        ArgumentNullException.ThrowIfNull(words);

        var tags = _wordSizesGetter
            .GetSizes(words)
            .Select(unplacedTag => new Tag(unplacedTag.Word, _cloudLayouter.PutNextRectangle(unplacedTag.Size)))
            .ToArray();

        var tagDrawings = GetTagDrawings(tags);
        return _tagsDrawer.Draw(tagDrawings);
    }

    private TagDrawing[] GetTagDrawings(IList<Tag> tags)
    {
        var tagDrawings = tags
            .Select(tag => new TagDrawing(tag, default!, default!, default))
            .ToArray();

        return _tagsSettingsSetters
            .Aggregate(tagDrawings, (tags, setter) => setter.Decorate(tags));
    }
}
