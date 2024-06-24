using BeeInventor.Core;
using BenchmarkDotNet.Attributes;

namespace BeeInventor.Benchmark;

public class SimpleDictionaryStage
{
    private const int Size = 10000;
    private readonly List<string> _list;
    private readonly SimpleDictionaryImpl _simpleDictionary;
    private readonly WildCardDictionaryImpl _wildCardDictionary;
    private readonly List<string> _inputs;

    public SimpleDictionaryStage()
    {
        _inputs = new List<string>();

        for (var i = 0; i < Size; i++)
        {
            _inputs.Add(Guid.NewGuid().ToString());
        }

        _simpleDictionary = new SimpleDictionaryImpl(_inputs);
        _wildCardDictionary = new WildCardDictionaryImpl(_inputs);
        _list = new List<string>(_inputs);
    }

    [Benchmark]
    public void find_word_in_list()
    {
        foreach (var word in _inputs)
        {
            var _ = _list.Contains(word);
        }

        for (var i = 0; i < Size; i++)
        {
            var word = Guid.NewGuid().ToString();
            var _ = _list.Contains(word);
        }
    }

    [Benchmark]
    public void find_word_in_simple_dictionary()
    {
        foreach (var word in _inputs)
        {
            var _ = _simpleDictionary.IsInDict(word);
        }

        for (var i = 0; i < Size; i++)
        {
            var word = Guid.NewGuid().ToString();
            var _ = _simpleDictionary.IsInDict(word);
        }
    }

    [Benchmark]
    public void find_word_in_simple_dictionary_with_wild_card()
    {
        foreach (var word in _inputs)
        {
            var _ = _wildCardDictionary.IsInDict(word);
        }

        for (var i = 0; i < Size; i++)
        {
            var word = Guid.NewGuid().ToString();
            var _ = _wildCardDictionary.IsInDict(word);
        }
    }
}