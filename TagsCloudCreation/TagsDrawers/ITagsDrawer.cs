using System.Drawing;

namespace TagsCloudCreation.TagsDrawers;

public interface ITagsDrawer
{
    public Image Draw(IList<TagDrawing> tags);
}
