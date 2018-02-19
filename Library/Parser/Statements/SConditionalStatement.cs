using System.Collections.Generic;
using Library.Graph;

namespace Library.Parser.Statements
{
    public sealed class SConditionalStatement : SStatement
    {
        public readonly SExpression Condition;

        public readonly SStatement ElseStatement;

        public readonly SStatement IfTrueStatement;

        public SConditionalStatement(SExpression condition, SStatement ifTrueStatement, SStatement elseStatement,
            SStatement nextStatement)
            : base(condition != null ? "if: " + condition.CodeString : "", nextStatement)
        {
            Condition = condition;

            IfTrueStatement = ifTrueStatement;
            ElseStatement = elseStatement;

            IfTrueStatement.SetNextStatement(null);
            ElseStatement.SetNextStatement(null);
        }

        public SConditionalStatement(SExpression condition, SStatement trueStatement, SStatement nextStatement)
            : base(condition != null ? "if: " + condition.CodeString : "", nextStatement)
        {
            Condition = condition;
            IfTrueStatement = trueStatement;
            IfTrueStatement.SetNextStatement(null);
        }

        public override List<GraphNode<SStatement>> BuildGraphNodes(Graph<SStatement> graph,
            List<GraphNode<SStatement>> previousNodes)
        {
            var currentNode = graph.AddNode(new GraphNode<SStatement>(this));
            foreach (var node in previousNodes)
                graph.AddDirectedEdge(node, currentNode, 1);

            var currentNodes = new List<GraphNode<SStatement>> {currentNode};
            var ntrue = IfTrueStatement.BuildGraphNodes(graph, currentNodes);

            if (ElseStatement != null)
            {
                var nfalse = ElseStatement.BuildGraphNodes(graph, currentNodes);
                ntrue.AddRange(nfalse);
            }
            else
            {
                ntrue.Add(currentNode);
            }

            previousNodes = NextStatement != null ? NextStatement.BuildGraphNodes(graph, ntrue) : ntrue;

            return previousNodes;
        }


        public override bool Contains(SStatement statement)
        {
            return statement == this || IfTrueStatement != null && IfTrueStatement.Contains(statement) ||
                   ElseStatement != null && ElseStatement.Contains(statement);
        }
    }
}