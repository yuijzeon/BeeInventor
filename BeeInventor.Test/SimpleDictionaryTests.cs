using BeeInventor.Core;

namespace BeeInventor.Test;

public class SimpleDictionaryTests
{
    [Test]
    public void example_from_pdf()
    {
        var dictionary = new SimpleDictionaryImpl(["cat", "car", "bar"]);
        dictionary.IsInDict("cat").Should().BeTrue();
        dictionary.IsInDict("bat").Should().BeFalse();
    }

    [Test]
    public void find_equals_word_in_simple_dictionary()
    {
        const int size = 100000;
        var words = new List<string>();

        for (var i = 0; i < size; i++)
        {
            words.Add(Guid.NewGuid().ToString());
        }

        var dictionary = new SimpleDictionaryImpl(words);

        foreach (var word in words)
        {
            dictionary.IsInDict(word).Should().BeTrue();
        }
    }

    [Test]
    public void find_not_equals_word_in_simple_dictionary()
    {
        const int size = 100000;
        var words = new List<string>();

        for (var i = 0; i < size; i++)
        {
            words.Add(Guid.NewGuid().ToString());
        }

        var dictionary = new SimpleDictionaryImpl(words);

        for (var i = 0; i < size; i++)
        {
            var word = Guid.NewGuid().ToString();
            dictionary.IsInDict(word).Should().BeFalse();
        }
    }
}