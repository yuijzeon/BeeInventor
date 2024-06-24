namespace BeeInventor.Core;

public class WildCardDictionaryImpl : Dictionary
{
    private readonly TrieNode _rootNode;

    public WildCardDictionaryImpl(List<string> words)
    {
        _rootNode = new TrieNode();

        foreach (var word in words)
        {
            var currentNode = _rootNode;

            foreach (var letter in word)
            {
                currentNode = currentNode.FindOrCreate(letter);
            }

            currentNode.MarkAsEnd();
        }
    }

    public bool IsInDict(string word)
    {
        return Search(_rootNode, word);
    }

    private static bool Search(TrieNode node, string word)
    {
        if (word == string.Empty)
        {
            return node.Children.Exists(x => x == null);
        }

        foreach (var nextNode in node.GetChildren())
        {
            if (word[0] == nextNode.Char || word[0] == '*')
            {
                return Search(nextNode, word[1..]);
            }
        }

        return false;
    }

    public class TrieNode
    {
        public char Char { get; set; }
        public List<TrieNode?> Children { get; set; } = [];

        public TrieNode FindOrCreate(char letter)
        {
            var nextNode = Children.Find(x => x?.Char == letter);

            if (nextNode != null)
            {
                return nextNode;
            }

            var newNode = new TrieNode { Char = letter };
            Children.Add(newNode);
            return newNode;
        }

        public void MarkAsEnd()
        {
            Children.Add(null);
        }

        public List<TrieNode> GetChildren()
        {
            var result = new List<TrieNode>();

            foreach (var child in Children)
            {
                if (child == null)
                {
                    continue;
                }

                result.Add(child);
            }

            return result;
        }
    }
}