using WordsFiltration.Configs;

namespace WordsFiltration.WordsSelectors;

public class WordsFilter : IWordsSelector
{
    private readonly IWordsSelectionConfig _wordsSelectionConfig;

    public WordsFilter(IWordsSelectionConfig wordsSelectionConfig)
    {
        ArgumentNullException.ThrowIfNull(wordsSelectionConfig);

        _wordsSelectionConfig = wordsSelectionConfig;
    }

    public IEnumerable<string> Select(IEnumerable<string> words)
    {
        ArgumentNullException.ThrowIfNull(words);

        var excludedWords = _wordsSelectionConfig.ExcludedWords?.ToHashSet();

        if (excludedWords == null)
        {
            return words;
        }

        return words.Where(word => !excludedWords.Contains(word));
    }
}
