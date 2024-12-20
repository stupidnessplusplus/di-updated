using FakeItEasy;
using FluentAssertions;
using System.Drawing;
using TagsCloudCreation.Configs;
using TagsCloudCreation.WordSizesGetters;

namespace TagsCloudCreation_Tests.WordSizesGetters;

[TestFixture]
internal class WordSizesByFrequencyGetterTests
{
    private const int ConfigMinSize = 8;
    private const string ConfigFontName = "Arial";
    private const FontStyle ConfigFontStyle = FontStyle.Regular;

    private ITagsFontConfig _tagsFontConfig;
    private IWordSizesGetterConfig _wordSizesGetterConfig;
    private WordSizesByFrequencyGetter _wordSizesGetter;

    [SetUp]
    public void SetUp()
    {
        _wordSizesGetterConfig = A.Fake<IWordSizesGetterConfig>();
        A.CallTo(() => _wordSizesGetterConfig.MinSize).Returns(ConfigMinSize);

        _tagsFontConfig = A.Fake<ITagsFontConfig>();
        A.CallTo(() => _tagsFontConfig.FontName).Returns(ConfigFontName);
        A.CallTo(() => _tagsFontConfig.FontStyle).Returns(ConfigFontStyle);

        _wordSizesGetter = new WordSizesByFrequencyGetter(_wordSizesGetterConfig, _tagsFontConfig);
    }

    [Test]
    public void Constructor_ThrowsException_WhenWordSizesGetterConfigIsNull()
    {
        var ctor = () => new WordSizesByFrequencyGetter(null!, _tagsFontConfig);
        ctor.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_ThrowsException_WhenTagsFontConfigIsNull()
    {
        var ctor = () => new WordSizesByFrequencyGetter(_wordSizesGetterConfig, null!);
        ctor.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void GetSizes_ThrowsException_WhenWordsListIsNull()
    {
        var getSizes = () => _wordSizesGetter.GetSizes(null!);
        getSizes.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void GetSizes_ReturnsTagWithHeightEqualToConfigMinSize_IfWordOccursOnce()
    {
        var words = new[] { "a" };

        var unplacedTags = _wordSizesGetter.GetSizes(words);

        unplacedTags.Should().HaveCount(1);
        unplacedTags[0].Size.Height.Should().Be(ConfigMinSize);
    }

    [Test]
    public void GetSizes_ReturnsTags_WithHeightsIncreasedByWordOccurencesNumber()
    {
        var words = new[] { "a", "b", "b", "c", "c", "c", "c" };
        
        var unplacedTags = _wordSizesGetter.GetSizes(words);

        unplacedTags.Should().HaveCount(3);
        unplacedTags.Should()
            .AllSatisfy(tag => tag.Size.Height.Should().Be(ConfigMinSize + words.Count(word => word == tag.Word) - 1));
    }

    [Test]
    public void GetSizes_ReturnsTags_SortedByHeightInDescendingOrder()
    {
        var words = new[] { "d", "a", "c", "c", "b", "d", "b", "d" };

        var unplacedTags = _wordSizesGetter.GetSizes(words);

        for (var i = 1; i < unplacedTags.Length; i++)
        {
            var currentHeight = unplacedTags[i].Size.Height;
            var previousHeight = unplacedTags[i - 1].Size.Height;
            currentHeight.Should().BeLessThanOrEqualTo(previousHeight);
        }
    }
}
