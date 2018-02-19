using System.Collections.Generic;
using Library.Graph;

namespace Library.Parser.Statements
{
    public abstract class SStatement
    {
        public readonly string CodeString;
        private bool _nextStatementSet;
        public bool Visible = true;

        public SStatement(string codeString, SStatement nextStatement)
        {
            CodeString = codeString;
            NextStatement = nextStatement;
        }

        public SStatement NextStatement { get; private set; }

        public abstract List<GraphNode<SStatement>> BuildGraphNodes(Graph<SStatement> graph,
            List<GraphNode<SStatement>> graphNodes);

        public abstract bool Contains(SStatement statement);

        public SStatement GetStatement()
        {
            return this;
        }

        public virtual void SetNextStatement(SStatement nextStatement)
        {
            if (_nextStatementSet) return;
            NextStatement = nextStatement;
            _nextStatementSet = true;
        }

        public override string ToString()
        {
            return "/* Statement: */ " + CodeString;
        }
    }
}