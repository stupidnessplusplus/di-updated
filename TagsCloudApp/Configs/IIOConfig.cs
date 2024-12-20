using System.Drawing.Imaging;

namespace TagsCloudApp.Configs;

public interface IIOConfig
{
    public string InputPath { get; }

    public string OutputPath { get; }

    public OutputImageFormat OutputImageFormat { get; }

    internal ImageFormat ImageFormat
    {
        get
        {
            return OutputImageFormat switch
            {
                OutputImageFormat.Png => ImageFormat.Png,
                OutputImageFormat.Jpeg => ImageFormat.Jpeg,
                OutputImageFormat.Bmp => ImageFormat.Bmp,
                _ => throw new ArgumentException("Unsupported image format.")
            };
        }
    }
}
