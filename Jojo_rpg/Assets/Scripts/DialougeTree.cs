using System.Collections.Generic;
using UnityEngine;

public class DialougeTree
{
    public class DialougeNode
    {
        public string speaker;
        public string text;
        public List<DialougeNode> children = new List<DialougeNode>();

        public DialougeNode(string _speaker, string _text)
        {
            speaker = _speaker;
            text = _text;
        }

        public DialougeNode AddChild(DialougeNode child)
        {
            children.Add(child);
            return child;
        }
    }

    public DialougeNode root { get; private set; }
    public DialougeNode currentNode { get; private set; }

    public DialougeTree(DialougeNode _root)
    {
        root = _root;
        currentNode = root;
    }


}
