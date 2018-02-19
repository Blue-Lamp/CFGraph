using System.Collections.Generic;

namespace Library.Graph
{
    public class GraphNode<T> : Node<T>
    {
        private List<int> _costs;

        public GraphNode()
        {
        }

        public GraphNode(T value) : base(value)
        {
        }

        public GraphNode(T value, NodeList<T> neighbors) : base(value, neighbors)
        {
        }

        public new NodeList<T> Neighbors => base.Neighbors ?? (base.Neighbors = new NodeList<T>());
        public List<int> Costs => _costs ?? (_costs = new List<int>());
    }
}