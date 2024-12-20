using FakeItEasy;
using FluentAssertions;
using System.Drawing;
using TagsCloudCreation.Configs;
using TagsCloudCreation.TagsDrawers;

namespace TagsCloudCreation_Tests.TagsDrawers;

[TestFixture]
internal class TagsDrawerTests
{
    private static readonly Color _backgroundColor = Color.FromArgb(255, 255, 255);

    private ITagsColorConfig _tagsColorConfig;
    private ITagsDrawer _tagsDrawer;

    [SetUp]
    public void SetUp()
    {
        _tagsColorConfig = A.Fake<ITagsColorConfig>();
        A.CallTo(() => _tagsColorConfig.BackgroundColor).Returns(_backgroundColor);

        _tagsDrawer = new TagsDrawer(_tagsColorConfig);
    }

    [Test]
    public void Constructor_ThrowsException_WhenTagsColorConfigIsNull()
    {
        var ctor = () => new TagsDrawer(null!);
        ctor.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Draw_ThrowsException_WhenTagsListIsNull()
    {
        var decorate = () => _tagsDrawer.Draw(null!);
        decorate.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Draw_Returns1x1Image_WhenTagsListIsEmpty()
    {
        var expectedSize = new Size(1, 1);

        var image = _tagsDrawer.Draw([]);

        image.Size.Should().Be(expectedSize);
    }

    [Test]
    public void Draw_SetsImageBackgroundColor()
    {
        var image = _tagsDrawer.Draw([]);

        image.GetPixel(0, 0).Should().Be(_backgroundColor);
    }
}
