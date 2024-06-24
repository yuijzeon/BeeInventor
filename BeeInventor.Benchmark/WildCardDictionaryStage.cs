using System.Text.RegularExpressions;
using BeeInventor.Core;
using BenchmarkDotNet.Attributes;

namespace BeeInventor.Benchmark;

public class WildCardDictionaryStage
{
    private const int Size = 100;
    private readonly List<string> _inputs;
    private readonly List<string> _list;
    private readonly WildCardDictionaryImpl _wildCardDictionary;

    public WildCardDictionaryStage()
    {
        var words = new List<string>();

        for (var i = 0; i < Size; i++)
        {
            words.Add(Guid.NewGuid().ToString());
        }

        _list = new List<string>(words);
        _wildCardDictionary = new WildCardDictionaryImpl(words);

        _inputs = words.Select((word, i) =>
        {
            var charIndex = i % word.Length;
            return word[..charIndex] + "*" + word[(charIndex + 1)..];
        }).ToList();
    }

    [Benchmark]
    public void find_wild_card_word_in_list()
    {
        foreach (var input in _inputs)
        {
            var _ = _list.Exists(word => new Regex($"^{input.Replace("*", ".*")}$").IsMatch(word));
        }
    }

    [Benchmark]
    public void find_word_in_wild_card_dictionary_with_wild_card()
    {
        foreach (var input in _inputs)
        {
            var _ = _wildCardDictionary.IsInDict(input);
        }
    }
}