using BeeInventor.Core;

namespace BeeInventor.Test;

public class WildCardDictionaryTests
{
    [Test]
    public void example_from_pdf()
    {
        var dictionary = new WildCardDictionaryImpl(["cat", "car", "bar"]);
        dictionary.IsInDict("cat").Should().BeTrue();
        dictionary.IsInDict("bat").Should().BeFalse();
        dictionary.IsInDict("*at").Should().BeTrue();
        dictionary.IsInDict("cr*").Should().BeFalse();
    }

    [Test]
    public void find_equals_word_in_wild_card_dictionary()
    {
        const int size = 1000;
        var words = new List<string>();

        for (var i = 0; i < size; i++)
        {
            words.Add(Guid.NewGuid().ToString());
        }

        var dictionary = new WildCardDictionaryImpl(words);

        foreach (var word in words)
        {
            dictionary.IsInDict(word).Should().BeTrue();
        }
    }

    [Test]
    public void find_not_equals_word_in_wild_card_dictionary()
    {
        const int size = 1000;
        var words = new List<string>();

        for (var i = 0; i < size; i++)
        {
            words.Add(Guid.NewGuid().ToString());
        }

        var dictionary = new WildCardDictionaryImpl(words);

        for (var i = 0; i < size; i++)
        {
            var word = Guid.NewGuid().ToString();
            dictionary.IsInDict(word).Should().BeFalse();
        }
    }
}