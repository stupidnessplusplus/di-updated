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

    private ITagsFontConfig tagsFontConfig;
    private IWordSizesGetterConfig wordSizesGetterConfig;
    private WordSizesByFrequencyGetter wordSizesGetter;

    [SetUp]
    public void SetUp()
    {
        wordSizesGetterConfig = A.Fake<IWordSizesGetterConfig>();
        A.CallTo(() => wordSizesGetterConfig.MinSize).Returns(ConfigMinSize);

        tagsFontConfig = A.Fake<ITagsFontConfig>();
        A.CallTo(() => tagsFontConfig.FontName).Returns(ConfigFontName);
        A.CallTo(() => tagsFontConfig.FontStyle).Returns(ConfigFontStyle);

        wordSizesGetter = new WordSizesByFrequencyGetter(wordSizesGetterConfig, tagsFontConfig);
    }

    [Test]
    public void Constructor_ThrowsException_WhenWordSizesGetterConfigIsNull()
    {
        var ctor = () => new WordSizesByFrequencyGetter(null!, tagsFontConfig);
        ctor.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Constructor_ThrowsException_WhenTagsFontConfigIsNull()
    {
        var ctor = () => new WordSizesByFrequencyGetter(wordSizesGetterConfig, null!);
        ctor.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void GetSizes_ThrowsException_WhenWordsListIsNull()
    {
        var getSizes = () => wordSizesGetter.GetSizes(null!);
        getSizes.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void GetSizes_ReturnsTagWithHeightEqualToConfigMinSize_IfWordOccursOnce()
    {
        var words = new[] { "a" };

        var unplacedTags = wordSizesGetter.GetSizes(words);

        unplacedTags.Should().HaveCount(1);
        unplacedTags[0].Size.Height.Should().Be(ConfigMinSize);
    }

    [Test]
    public void GetSizes_ReturnsTags_WithHeightsIncreasedByWordOccurencesNumber()
    {
        var words = new[] { "a", "b", "b", "c", "c", "c", "c" };
        
        var unplacedTags = wordSizesGetter.GetSizes(words);

        unplacedTags.Should().HaveCount(3);
        unplacedTags.Should()
            .AllSatisfy(tag => tag.Size.Height.Should().Be(ConfigMinSize + words.Count(word => word == tag.Word) - 1));
    }

    [Test]
    public void GetSizes_ReturnsTags_SortedByHeightInDescendingOrder()
    {
        var words = new[] { "d", "a", "c", "c", "b", "d", "b", "d" };

        var unplacedTags = wordSizesGetter.GetSizes(words);

        var unplacedTagsHeights = unplacedTags.Select(tag => tag.Size.Height);
        unplacedTagsHeights.Should().BeInDescendingOrder();
    }
}
