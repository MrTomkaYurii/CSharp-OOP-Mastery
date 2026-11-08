using System.Collections.Generic;

namespace AutoMapperDemo.Domain
{
    // Приклад для демонстрації PreserveReferences / MaxDepth
    public class Node
    {
        public string Name { get; set; } = "";
        public Node? Parent { get; set; }
        public List<Node> Children { get; set; } = new();
    }
}