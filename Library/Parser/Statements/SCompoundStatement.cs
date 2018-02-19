using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Library.Graph;

namespace Library.Parser.Statements
{
    public sealed class SCompoundStatement : SStatement, IEnumerable<SStatement>
    {
        private readonly SStatement[] _statementList;

        public SCompoundStatement(SStatement[] statementList, SStatement nextStatement)
            : base("{ /* ... */ }", nextStatement)
        {
            _statementList = (SStatement[]) statementList.Clone();
            Visible = false;
        }

        public SStatement this[int i] => _statementList[i];

        public IEnumerator<SStatement> GetEnumerator()
        {
            return ((IEnumerable<SStatement>) _statementList).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _statementList.GetEnumerator();
        }

        public override List<GraphNode<SStatement>> BuildGraphNodes(Graph<SStatement> graph,
            List<GraphNode<SStatement>> previousNodes)
        {
            var currentNode = graph.AddNode(new GraphNode<SStatement>(this));
            foreach (var node in previousNodes)
                graph.AddDirectedEdge(node, currentNode, 1);

            previousNodes = new List<GraphNode<SStatement>> {currentNode};

            if (_statementList.Length > 0 && _statementList[0] != null)
                previousNodes = _statementList[0].BuildGraphNodes(graph, previousNodes);

            if (NextStatement != null)
                previousNodes = NextStatement.BuildGraphNodes(graph, previousNodes);

            return previousNodes;
        }

        public override bool Contains(SStatement statement)
        {
            return statement == this ||
                   _statementList.Any(currStatement => currStatement != null && currStatement.Contains(statement));
        }
    }
}