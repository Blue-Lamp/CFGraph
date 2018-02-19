using System.Collections.Generic;
using Library.Graph;

namespace Library.Parser.Statements
{
    public sealed class SLabeledStatement : SStatement
    {
        private readonly List<SJumpStatement> _jumpOrigins = new List<SJumpStatement>();
        public readonly SExpression CaseExpression;
        public readonly SExpression ReturnedExpression;
        private bool _jumpOrigintSet;
        private bool _labeledStatementSet;

        public SLabeledStatement(string codeString, SExpression caseExpression, SStatement labeledStatement)
            : base(caseExpression != null ? "case " + caseExpression.CodeString : codeString,
                labeledStatement?.NextStatement)
        {
            LabeledStatement = labeledStatement;
            CaseExpression = caseExpression;
        }

        public SStatement LabeledStatement { get; private set; }

        public void AddJumpOrigin(SJumpStatement targetStatement)
        {
            _jumpOrigins.Add(targetStatement);
            _jumpOrigintSet = true;
        }

        public override List<GraphNode<SStatement>> BuildGraphNodes(Graph<SStatement> graph,
            List<GraphNode<SStatement>> previousNodes)
        {
            var currentNode = graph.AddNode(new GraphNode<SStatement>(this));
            foreach (var node in previousNodes)
                graph.AddDirectedEdge(node, currentNode, 1);

            var currentNodes = new List<GraphNode<SStatement>> {currentNode};
            return LabeledStatement != null ? LabeledStatement.BuildGraphNodes(graph, currentNodes) : currentNodes;
        }


        public override bool Contains(SStatement statement)
        {
            return statement == this ||
                   LabeledStatement != null &&
                   LabeledStatement.Contains(statement);
        }

        public void SetLabeledStatement(SStatement labeledStatement)
        {
            if (_labeledStatementSet) return;
            LabeledStatement = labeledStatement;
            _labeledStatementSet = true;
        }

        public override void SetNextStatement(SStatement nextStatement)
        {
            LabeledStatement.SetNextStatement(nextStatement);
        }
    }
}