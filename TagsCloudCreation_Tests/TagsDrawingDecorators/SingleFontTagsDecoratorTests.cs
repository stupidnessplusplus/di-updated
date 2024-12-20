using FakeItEasy;
using FluentAssertions;
using System.Drawing;
using TagsCloudCreation.Configs;
using TagsCloudCreation.TagsDrawingDecorators;

namespace TagsCloudCreation_Tests.TagsDrawingDecorators;

[TestFixture]
internal class SingleFontTagsDecoratorTests : TagsDrawingDecoratorTests
{
    private const string ConfigFontName = "Arial";
    private const FontStyle ConfigFontStyle = FontStyle.Regular;

    private ITagsFontConfig _tagsFontConfig;

    [SetUp]
    public void SetUp()
    {
        _tagsFontConfig = A.Fake<ITagsFontConfig>();
        A.CallTo(() => _tagsFontConfig.FontName).Returns(ConfigFontName);
        A.CallTo(() => _tagsFontConfig.FontStyle).Returns(ConfigFontStyle);

        _tagsDecorator = new SingleFontTagsDecorator(_tagsFontConfig);
    }

    [Test]
    public void Constructor_ThrowsException_WhenTagsFontConfigIsNull()
    {
        var ctor = () => new SingleFontTagsDecorator(null!);
        ctor.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Decorate_SetsTagsFontNameAndFontStyle()
    {
        var expectedTags = _tags
            .Select(tag => tag with
            {
                FontName = ConfigFontName,
                FontStyle = ConfigFontStyle,
            });

        var decoratedTags = _tagsDecorator.Decorate(_tags);

        decoratedTags.Should().BeEquivalentTo(expectedTags);
    }
}
