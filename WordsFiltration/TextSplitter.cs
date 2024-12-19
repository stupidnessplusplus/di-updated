using System.Text.RegularExpressions;

namespace WordsFiltration;

public class TextSplitter
{
    public string[] SplitToWords(string text)
    {
        text = text.ToLower();
        return Regex.Split(text, @"[\p{P}\s]+");
    }
}
