using FluentAssertions;
using System.Drawing;
using TagsCloudCreation;
using TagsCloudCreation.TagsDrawingDecorators;

namespace TagsCloudCreation_Tests.TagsDrawingDecorators;

[TestFixture]
internal abstract class TagsDrawingDecoratorTests
{
    protected static readonly TagDrawing[] tags =
    [
        new TagDrawing(new Tag("a", default), default!, default!, default),
        new TagDrawing(new Tag("b", default), default!, "Century", FontStyle.Italic),
        new TagDrawing(new Tag("c", default), Brushes.White, default!, default),
        new TagDrawing(new Tag("d", default), Brushes.White, "Century", FontStyle.Italic),
    ];

    protected ITagsDrawingDecorator tagsDecorator = null!;

    [Test]
    public void Decorate_ThrowsException_WhenTagsListIsNull()
    {
        var decorate = () => tagsDecorator.Decorate(null!);
        decorate.Should().Throw<ArgumentNullException>();
    }

    [Test]
    public void Decorate_CreatesNewListAndTagDrawings()
    {
        var decoratedTags = tagsDecorator.Decorate(tags);

        decoratedTags.Should().NotBeSameAs(tags);
        decoratedTags.Should()
            .AllSatisfy(decoratedTag => tags.Should()
                .AllSatisfy(tag => decoratedTag.Should().NotBeSameAs(tag)));
    }

    [Test]
    public void Decorate_KeepsTagsOrder()
    {
        var decoratedTags = tagsDecorator.Decorate(tags);

        decoratedTags.Should().HaveCount(tags.Length);
        decoratedTags.Zip(tags).Should()
            .AllSatisfy(tagsPair => tagsPair.First.Tag.Should().Be(tagsPair.Second.Tag));
    }
}
