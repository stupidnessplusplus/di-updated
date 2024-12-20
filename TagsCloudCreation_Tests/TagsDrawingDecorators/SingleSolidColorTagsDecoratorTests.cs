using FakeItEasy;
using FluentAssertions;
using System.Drawing;
using TagsCloudCreation.Configs;
using TagsCloudCreation.TagsDrawingDecorators;

namespace TagsCloudCreation_Tests.TagsDrawingDecorators;

[TestFixture]
internal class SingleSolidColorTagsDecoratorTests : TagsDrawingDecoratorTests
{
    private static readonly Color _configMainColor = Color.FromArgb(0, 0, 0);

    private ITagsColorConfig _tagsColorConfig;

    [SetUp]
    public void SetUp()
    {
        _tagsColorConfig = A.Fake<ITagsColorConfig>();
        A.CallTo(() => _tagsColorConfig.MainColor).Returns(_configMainColor);

        _tagsDecorator = new SingleSolidColorTagsDecorator(_tagsColorConfig);
    }

    [Test]
    public void Constructor_ThrowsException_WhenTagsColorConfigIsNull()
    {
        var ctor = () => new SingleSolidColorTagsDecorator(null!);
        ctor.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Decorate_SetsTagsBrush()
    {
        var brush = new SolidBrush(_configMainColor);
        var expectedTags = _tags
            .Select(tag => tag with { Brush = brush });

        var decoratedTags = _tagsDecorator.Decorate(_tags);

        decoratedTags.Should().BeEquivalentTo(expectedTags);
    }
}
