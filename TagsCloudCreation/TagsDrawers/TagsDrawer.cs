using System.Drawing;
using TagsCloudCreation.Configs;

namespace TagsCloudCreation.TagsDrawers;

public class TagsDrawer : ITagsDrawer
{
    private readonly ITagsColorConfig _colorConfig;

    public TagsDrawer(ITagsColorConfig colorConfig)
    {
        _colorConfig = colorConfig;
    }

    public Image Draw(IList<TagDrawing> tagsWithSettings)
    {
        ArgumentNullException.ThrowIfNull(tagsWithSettings);

        var imageSize = GetImageSizeToFitTags(tagsWithSettings);
        var image = new Bitmap(imageSize.Width, imageSize.Height);

        FillBackground(image, _colorConfig.BackgroundColor);
        DrawTags(image, tagsWithSettings);

        return image;
    }

    private Size GetImageSizeToFitTags(IList<TagDrawing> tagsWithSettings)
    {
        var width = 2 * tagsWithSettings
            .Max(tag => Math.Max(Math.Abs(tag.Tag.Rectangle.Left), tag.Tag.Rectangle.Right));
        var height = 2 * tagsWithSettings
            .Max(tag => Math.Max(Math.Abs(tag.Tag.Rectangle.Top), tag.Tag.Rectangle.Bottom));
        return new Size(width, height);
    }

    private void FillBackground(Image image, Color color)
    {
        using var graphics = Graphics.FromImage(image);
        using var brush = new SolidBrush(color);
        graphics.FillRectangle(brush, new Rectangle(Point.Empty, image.Size));
    }

    private void DrawTags(Image image, IList<TagDrawing> tags)
    {
        using var graphics = Graphics.FromImage(image);

        foreach (var tag in CenterTags(image, tags))
        {
            tag.Draw(graphics);
        }
    }

    private IEnumerable<TagDrawing> CenterTags(Image image, IList<TagDrawing> tags)
    {
        var delta = new Size(image.Width / 2, image.Height / 2);

        return tags
            .Select(tagDrawing => tagDrawing with
            {
                Tag = tagDrawing.Tag with
                {
                    Rectangle = tagDrawing.Tag.Rectangle with
                    {
                        Location = tagDrawing.Tag.Rectangle.Location + delta,
                    },
                },
            });
    }
}
