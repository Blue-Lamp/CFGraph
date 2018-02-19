using System.Collections.Generic;
using Library.Graph;

namespace Library.Parser.Statements
{
    public sealed class SJumpStatement : SStatement
    {
        public readonly SExpression ReturnedExpression;
        public readonly string TargetIdentifier;
        private bool _targetStatementSet;

        public SJumpStatement(string codeString, SStatement nextStatement)
            : base(codeString, nextStatement)
        {
            var parts = codeString.Split(' ');

            if (parts[0] == "goto" && parts.Length > 1)
                TargetIdentifier = parts[1];
            else
                TargetIdentifier = null;

            ReturnedExpression = null;
        }

        public SJumpStatement(SExpression returnedExpression, SStatement nextStatement)
            : base("return " + returnedExpression.CodeString, nextStatement)
        {
            TargetIdentifier = null;
            ReturnedExpression = returnedExpression;
        }

        public SLabeledStatement TargetStatement { get; private set; }

        public override List<GraphNode<SStatement>> BuildGraphNodes(Graph<SStatement> graph,
            List<GraphNode<SStatement>> previousNodes)
        {
            TargetStatement?.AddJumpOrigin(this);

            var currentNode = graph.AddNode(new GraphNode<SStatement>(this));
            foreach (var node in previousNodes)
                graph.AddDirectedEdge(node, currentNode, 1);

            previousNodes = new List<GraphNode<SStatement>> {currentNode};

            var findedNode = (GraphNode<SStatement>) graph.Nodes.FindByValue(TargetStatement);
            graph.AddDirectedEdge(currentNode, findedNode, 1);

            return NextStatement != null
                ? NextStatement.BuildGraphNodes(graph, previousNodes)
                : new List<GraphNode<SStatement>>();
        }

        public override bool Contains(SStatement statement)
        {
            return statement == this;
        }

        public void SetTargetStatement(SLabeledStatement targetStatement)
        {
            if (_targetStatementSet) return;
            TargetStatement = targetStatement;
            _targetStatementSet = true;
        }
    }
}