using System.Collections.Generic;
using Library.Graph;

namespace Library.Parser.Statements
{
    public sealed class SFunctionDefinition
    {
        private readonly SCompoundStatement _functionBody;
        private readonly string _functionName;

        public SFunctionDefinition(string functionName, SCompoundStatement functionBody)
        {
            _functionName = functionName;
            _functionBody = functionBody;
        }

        public void BuildGraphNodes(Graph<SStatement> graph)
        {
            var lastNodes = _functionBody.BuildGraphNodes(graph, new List<GraphNode<SStatement>>());
            var currentNode = graph.AddNode(new GraphNode<SStatement>(new SLabeledStatement("EndNode", null, null)));

            foreach (var node in lastNodes)
                graph.AddDirectedEdge(node, currentNode, 1);
        }
    }
}