using System.Collections.Generic;
using Library.Graph;

namespace Library.Parser.Statements
{
    public sealed class SExpressionStatement : SStatement
    {
        private readonly SExpression _expression;

        public SExpressionStatement(SExpression expression, SStatement nextStatement)
            : base(expression.CodeString, nextStatement)
        {
            _expression = expression;
        }

        public override List<GraphNode<SStatement>> BuildGraphNodes(Graph<SStatement> graph,
            List<GraphNode<SStatement>> previousNodes)
        {
            var currentNode = graph.AddNode(new GraphNode<SStatement>(this));
            foreach (var node in previousNodes)
                graph.AddDirectedEdge(node, currentNode, 1);

            previousNodes = new List<GraphNode<SStatement>> {currentNode};

            if (NextStatement != null)
                previousNodes = NextStatement.BuildGraphNodes(graph, previousNodes);

            return previousNodes;
        }


        public override bool Contains(SStatement statement)
        {
            return statement == this;
        }
    }
}