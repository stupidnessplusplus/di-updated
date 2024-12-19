using System.Drawing;

namespace TagsCloudCreation;

public record TagDrawing(
    Tag Tag,
    Brush Brush,
    string FontName,
    FontStyle FontStyle)
{
    public void Draw(Graphics graphics)
    {
        using var font = new Font(FontName, Tag.Rectangle.Height, FontStyle, GraphicsUnit.Pixel);
        graphics.DrawString(Tag.Word, font, Brush, Tag.Rectangle.Location);
    }
}
