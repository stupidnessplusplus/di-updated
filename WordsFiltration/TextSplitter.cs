using System.Text.RegularExpressions;
using WordsFiltration.WordsSelectors;

namespace WordsFiltration;

public class TextSplitter
{
    private static readonly Regex _wordSplitRegex = new Regex(@"[\p{P}\s-[-]]+");

    private readonly IEnumerable<IWordsSelector> _wordsSelectors;

    public TextSplitter(IEnumerable<IWordsSelector> wordsSelectors)
    {
        ArgumentNullException.ThrowIfNull(wordsSelectors);

        _wordsSelectors = wordsSelectors;
    }

    public string[] SplitToWords(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        text = text.ToLower();

        var words = _wordSplitRegex
            .Split(text)
            .Where(word => !string.IsNullOrEmpty(word) && !word.All(ch => ch == '-'));

        return _wordsSelectors
            .Aggregate(words, (words, wordsSelector) => wordsSelector.Select(words))
            .ToArray();
    }
}
