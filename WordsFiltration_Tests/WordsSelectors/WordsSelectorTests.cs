using FluentAssertions;
using WordsFiltration.WordsSelectors;

namespace WordsFiltration_Tests.WordsSelectors;

internal abstract class WordsSelectorTests
{
    protected IWordsSelector wordSelector = null!;

    [Test]
    public void Select_ThrowsException_WhenWordsEnumerableIsNull()
    {
        var decorate = () => wordSelector.Select(null!);
        decorate.Should().Throw<ArgumentNullException>();
    }
}
