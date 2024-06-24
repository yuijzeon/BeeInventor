using BeeInventor.Core;
using BenchmarkDotNet.Attributes;

namespace BeeInventor.Benchmark;

public class SimpleDictionaryStage
{
    private const int Size = 10000;
    private readonly SimpleDictionaryImpl _dictionary;
    private readonly List<string> _list;
    private readonly List<string> _words;

    public SimpleDictionaryStage()
    {
        _words = new List<string>();

        for (var i = 0; i < Size; i++)
        {
            _words.Add(Guid.NewGuid().ToString());
        }

        _dictionary = new SimpleDictionaryImpl(_words);
        _list = new List<string>(_words);
    }

    [Benchmark]
    public void find_word_in_list()
    {
        foreach (var word in _words)
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
        foreach (var word in _words)
        {
            var _ = _dictionary.IsInDict(word);
        }

        for (var i = 0; i < Size; i++)
        {
            var word = Guid.NewGuid().ToString();
            var _ = _dictionary.IsInDict(word);
        }
    }
}