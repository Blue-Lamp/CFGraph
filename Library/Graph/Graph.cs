using System;
using System.Collections;
using System.Collections.Generic;

namespace Library.Graph
{
    public class Graph<T> : IEnumerable<T>
    {
        public Graph() : this(null)
        {
        }

        public Graph(NodeList<T> nodeSet)
        {
            Nodes = nodeSet ?? new NodeList<T>();
        }

        public NodeList<T> Nodes { get; }

        public int Count => Nodes.Count;

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void AddDirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
        {
            if (from == null || to == null)
                return;

            from.Neighbors.Add(to);
            from.Costs.Add(cost);
        }

        public void AddDirectedEdges(List<GraphNode<T>> from, GraphNode<T> to, int cost)
        {
            if (from == null || to == null)
                return;

            foreach (var node in from)
            {
                node.Neighbors.Add(to);
                node.Costs.Add(cost);
            }
        }

        public void AddDirectedEdges2(List<GraphNode<T>> from, List<GraphNode<T>> to, int cost)
        {
            if (from == null || to == null)
                return;

            foreach (var nodeFrom in from)
            foreach (var nodeTo in to)
            {
                nodeFrom.Neighbors.Add(nodeTo);
                nodeFrom.Costs.Add(cost);
            }
        }

        public GraphNode<T> AddNode(GraphNode<T> node)
        {
            Nodes.Add(node);
            return node;
        }

        public void AddUndirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
        {
            from.Neighbors.Add(to);
            from.Costs.Add(cost);

            to.Neighbors.Add(from);
            to.Costs.Add(cost);
        }

        public bool Contains(T value)
        {
            return Nodes.FindByValue(value) != null;
        }

        public int[,] GetMatrix()
        {
            //todo optimize
            var matrix = new List<List<int>>();

            foreach (var t1 in Nodes)
            {
                var list = new List<int>();
                for (var j = 0; j < Nodes.Count; j++)
                    list.Add(0);
                var gnode1 = (GraphNode<T>) t1;
                foreach (var t in Nodes)
                {
                    var gnode2 = (GraphNode<T>) t;
                    for (var k = 0; k < gnode1.Neighbors.Count; k++)
                        if (gnode1.Neighbors[k] == gnode2)
                            for (var l = 0; l < Nodes.Count; l++)
                                if (gnode2 == Nodes[l])
                                    list[l] = gnode1.Costs[k];
                }

                matrix.Add(list);
            }

            var ints = new int[Nodes.Count, matrix.Count];

            for (var i1 = 0; i1 < matrix.Count; i1++)
            {
                var list = matrix[i1];
                for (var i2 = 0; i2 < list.Count; i2++)
                {
                    var i = list[i2];
                    ints[i1, i2] = i;
                }
            }

            return ints;
        }

        public bool Remove(T value)
        {
            var nodeToRemove = (GraphNode<T>) Nodes.FindByValue(value);
            if (nodeToRemove == null)
                return false;

            Nodes.Remove(nodeToRemove);

            foreach (GraphNode<T> gnode in Nodes)
            {
                var index = gnode.Neighbors.IndexOf(nodeToRemove);
                if (index == -1) continue;
                gnode.Neighbors.RemoveAt(index);
                gnode.Costs.RemoveAt(index);
            }

            return true;
        }


        public bool RemoveByValueAndAddEdges(T value)
        {
            var nodeToRemove = (GraphNode<T>) Nodes.FindByValue(value);
            if (nodeToRemove == null)
                return false;

            var output = nodeToRemove.Neighbors;
            var input = new NodeList<T>();

            Nodes.Remove(nodeToRemove);

            foreach (GraphNode<T> gnode in Nodes)
            {
                var index = gnode.Neighbors.IndexOf(nodeToRemove);
                if (index == -1) continue;
                input.Add(gnode);
                gnode.Neighbors.RemoveAt(index);
                gnode.Costs.RemoveAt(index);
            }

            for (var i = output.Count - 1; i >= 0; i--)
            for (var j = input.Count - 1; j >= 0; j--)
                AddDirectedEdge((GraphNode<T>) input[j], (GraphNode<T>) output[i], 1);

            return true;
        }
    }
}