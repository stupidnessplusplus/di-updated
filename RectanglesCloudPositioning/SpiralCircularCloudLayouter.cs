using System.Drawing;

namespace RectanglesCloudPositioning;

public class SpiralCircularCloudLayouter
    : ICloudLayouter
{
    private readonly List<Rectangle> _rectangles = [];
    private readonly Point _center;

    private int _radius;

    public SpiralCircularCloudLayouter() : this(Point.Empty)
    {
    }

    public SpiralCircularCloudLayouter(Point center)
    {
        _center = center;
    }

    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rectangleSize.Width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(rectangleSize.Height);

        var rectangle = GetPositionOnCircleToPutRectangle(rectangleSize);
        _rectangles.Add(rectangle);
        return rectangle;
    }

    private Rectangle GetPositionOnCircleToPutRectangle(Size rectangleSize)
    {
        if (TryGetPositionOnCircleToPutRectangle(rectangleSize, out var putPosition))
        {
            return new Rectangle(putPosition, rectangleSize);
        }

        var previousSize = _rectangles[^1].Size;
        _radius += Math.Min(previousSize.Width + rectangleSize.Width, previousSize.Height + rectangleSize.Height) / 4;
        return GetPositionOnCircleToPutRectangle(rectangleSize);
    }

    private bool TryGetPositionOnCircleToPutRectangle(Size rectangleSize, out Point putPosition)
    {
        foreach (var point in GetCirclePoints(_radius, Point.Empty - rectangleSize / 2))
        {
            var rectangle = new Rectangle(point, rectangleSize);

            if (CanPut(rectangle))
            {
                putPosition = point;
                return true;
            }
        }

        putPosition = default;
        return false;
    }

    private bool CanPut(Rectangle rectangle)
    {
        return _rectangles
            .All(otherRectangle => !otherRectangle.IntersectsWith(rectangle));
    }

    private IEnumerable<Point> GetCirclePoints(int radius, Point center)
    {
        if (radius == 0)
        {
            yield return center;
            yield break;
        }

        var centerSize = (Size)center;
        var maxC = Math.Sqrt(2) / 2 * radius;
        var step = 1;

        for (var c = -maxC; c <= maxC; c += step)
        {
            var x = (int)c;
            var y = GetOtherCoordinateOnCircle(x, radius);

            yield return new Point(x, -y) + centerSize;
            yield return new Point(-y, -x) + centerSize;
            yield return new Point(-x, y) + centerSize;
            yield return new Point(y, x) + centerSize;
        }
    }

    private int GetOtherCoordinateOnCircle(int x, int radius)
    {
        // x ^ 2 + y ^ 2 = r ^ 2
        return (int)Math.Sqrt(radius * radius - x * x);
    }
}
