using System.Drawing;
using TagsCloudCreation.Configs;

namespace TagsCloudCreation.TagsDrawers;

public class TagsDrawer : ITagsDrawer
{
    private readonly ITagsColorConfig colorConfig;

    public TagsDrawer(ITagsColorConfig colorConfig)
    {
        ArgumentNullException.ThrowIfNull(colorConfig);

        this.colorConfig = colorConfig;
    }

    public Bitmap Draw(IList<TagDrawing> tagsWithSettings)
    {
        ArgumentNullException.ThrowIfNull(tagsWithSettings);

        var imageSize = GetImageSizeToFitTags(tagsWithSettings);
        var image = new Bitmap(imageSize.Width, imageSize.Height);

        FillBackground(image, colorConfig.BackgroundColor);
        DrawTags(image, tagsWithSettings);

        return image;
    }

    private Size GetImageSizeToFitTags(IList<TagDrawing> tagsWithSettings)
    {
        if (tagsWithSettings.Count == 0)
        {
            return new Size(1, 1);
        }

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

        foreach (var tag in CenterTags(image.Size, tags))
        {
            Draw(graphics, tag);
        }
    }

    private IEnumerable<TagDrawing> CenterTags(Size imageSize, IList<TagDrawing> tags)
    {
        var delta = new Size(imageSize.Width / 2, imageSize.Height / 2);

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

    private void Draw(Graphics graphics, TagDrawing tag)
    {
        using var font = new Font(tag.FontName, tag.Tag.Rectangle.Height, tag.FontStyle, GraphicsUnit.Pixel);
        graphics.DrawString(tag.Tag.Word, font, tag.Brush, tag.Tag.Rectangle.Location);
    }
}
