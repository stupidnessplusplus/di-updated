using System.Drawing;
using TagsCloudCreation.Configs;

namespace TagsCloudCreation.WordSizesGetters;

public class WordSizesByFrequencyGetter : IWordSizesGetter
{
    private readonly Image _emptyImage = new Bitmap(1, 1);

    private readonly IWordSizesGetterConfig _wordSizesGetterConfig;
    private readonly ITagsFontConfig _tagsFontConfig;

    public WordSizesByFrequencyGetter(IWordSizesGetterConfig wordSizesGetterConfig, ITagsFontConfig tagsFontConfig)
    {
        ArgumentNullException.ThrowIfNull(wordSizesGetterConfig);
        ArgumentNullException.ThrowIfNull(tagsFontConfig);

        _wordSizesGetterConfig = wordSizesGetterConfig;
        _tagsFontConfig = tagsFontConfig;
    }

    public UnplacedTag[] GetSizes(IList<string> words)
    {
        ArgumentNullException.ThrowIfNull(words);

        return words
            .Where(word => !string.IsNullOrWhiteSpace(word))
            .GroupBy(word => word)
            .Select(group => (Word: group.Key, Frequency: group.Count()))
            .OrderByDescending(x => x.Frequency)
            .Select(x => GetSize(x.Word, x.Frequency))
            .ToArray();
    }

    private UnplacedTag GetSize(string word, int wordFrequency)
    {
        var height = _wordSizesGetterConfig.MinSize + wordFrequency - 1;
        using var wordFont = new Font(_tagsFontConfig.FontName, height, _tagsFontConfig.FontStyle, GraphicsUnit.Pixel);
        using var graphics = Graphics.FromImage(_emptyImage);

        var sizeF = graphics.MeasureString(word, wordFont);
        var size = new Size((int)Math.Ceiling(sizeF.Width), height);
        return new UnplacedTag(word, size);
    }
}
