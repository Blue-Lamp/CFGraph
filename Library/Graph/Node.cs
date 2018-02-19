namespace Library.Graph
{
    public class Node<T>
    {
        public Node()
        {
        }

        public Node(T data) : this(data, new NodeList<T>())
        {
        }

        public Node(T data, NodeList<T> neighbors)
        {
            Value = data;
            Neighbors = neighbors;
        }

        public T Value { get; set; }
        public NodeList<T> Neighbors { get; set; }
    }
}