namespace BeeInventor.Core;

public class SimpleDictionaryImpl : Dictionary
{
    private readonly List<List<string>> _buckets;
    private readonly int _bucketsSize;

    public SimpleDictionaryImpl(List<string> words)
    {
        _bucketsSize = GetPrime(words.Count);
        _buckets = new List<List<string>>();

        for (var i = 0; i < _bucketsSize; i++)
        {
            _buckets.Add([]);
        }

        foreach (var word in words)
        {
            var index = GetHashIndex(word);

            if (!_buckets[index].Contains(word))
            {
                _buckets[index].Add(word);
            }
        }
    }

    public bool IsInDict(string word)
    {
        var index = GetHashIndex(word);
        return _buckets[index].Contains(word);
    }

    private int GetHashIndex(string word)
    {
        var index = word.GetHashCode() % _bucketsSize;

        if (index < 0)
        {
            return index + _bucketsSize;
        }

        return index;
    }

    private static int GetPrime(int n)
    {
        if (n < 2)
        {
            return n;
        }

        var isPrime = new List<bool>();

        for (var i = 0; i <= n; i++)
        {
            isPrime.Add(true);
        }

        for (var i = 2; i * i <= n; i++)
        {
            if (isPrime[i])
            {
                for (var j = i * i; j <= n; j += i)
                {
                    isPrime[j] = false;
                }
            }
        }

        return isPrime.LastIndexOf(true);
    }
}