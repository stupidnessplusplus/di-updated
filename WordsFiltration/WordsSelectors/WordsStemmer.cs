using MystemSharp;
using WordsFiltration.Configs;

namespace WordsFiltration.WordsSelectors;

public class WordsStemmer : IWordsSelector
{
    private readonly IWordsSelectionConfig _wordsSelectionConfig;

    public WordsStemmer(IWordsSelectionConfig wordsSelectionConfig)
    {
        ArgumentNullException.ThrowIfNull(wordsSelectionConfig);

        _wordsSelectionConfig = wordsSelectionConfig;
    }

    public IEnumerable<string> Select(IEnumerable<string> words)
    {
        ArgumentNullException.ThrowIfNull(words);

        return words.Select(word => new Analyses(word)[0].Text);
    }
}
