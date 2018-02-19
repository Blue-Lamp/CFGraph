using System.Collections.Generic;
using System.Linq;
using Library.Graph;

namespace Library.Parser.Statements
{
    public sealed class SSwitchStatement : SStatement
    {
        private readonly SExpression Condition;
        private readonly SLabeledStatement[] jumpTargets;
        private readonly SStatement SwitchStatement;

        public SSwitchStatement(SExpression condition, SStatement switchStatement,
            SLabeledStatement[] jumpTargets, SStatement nextStatement)
            : base(condition != null ? "switch: " + condition.CodeString : "",
                new SLabeledStatement("switch break target", null, nextStatement) {Visible = false})
        {
            Condition = condition;
            SwitchStatement = switchStatement;
            SwitchStatement.SetNextStatement(null);
            this.jumpTargets = (SLabeledStatement[]) jumpTargets.Clone();
        }

        public override List<GraphNode<SStatement>> BuildGraphNodes(Graph<SStatement> graph,
            List<GraphNode<SStatement>> previousNodes)
        {
            var currentNodes = new List<GraphNode<SStatement>>();
            var currentNode = graph.AddNode(new GraphNode<SStatement>(this));
            graph.AddDirectedEdges(previousNodes, currentNode, 1);

            var currentNodeList = new List<GraphNode<SStatement>> {currentNode};
            var nextNode = new List<GraphNode<SStatement>>();

            if (SwitchStatement != null)
                nextNode.AddRange(SwitchStatement.BuildGraphNodes(graph, currentNodeList));

            foreach (var jumpTarget in jumpTargets)
            {
                var a = FindNodes(graph, jumpTarget.NextStatement);
                if (a != null)
                    foreach (var graphNode in a)
                    {
                        graphNode.Neighbors.Clear();
                        graphNode.Costs.Clear();
                    }

                AddDirectedEdgeToElements(graph, currentNode.Value, jumpTarget.LabeledStatement);
                currentNodes.AddRange(FindNodes(graph, jumpTarget.NextStatement));
            }

            currentNodes.AddRange(nextNode);

            //todo: change default on enum
            if (jumpTargets.Count(q => q.CodeString == "default") == 0)
                currentNodes.Add(currentNode);

            if (NextStatement != null)
                currentNodes = new List<GraphNode<SStatement>>(NextStatement.BuildGraphNodes(graph, currentNodes));

            return currentNodes;
        }


        public override bool Contains(SStatement statement)
        {
            return statement == this ||
                   SwitchStatement != null &&
                   SwitchStatement.Contains(statement);
        }

        private void AddDirectedEdgeToElements(Graph<SStatement> graph, SStatement jumpTargetLabeledStatement,
            SStatement labeledStatement)
        {
            if (graph == null || jumpTargetLabeledStatement == null || labeledStatement == null)
                return;

            var a = FindNodes(graph, jumpTargetLabeledStatement.GetStatement());
            var b = FindNodes(graph, labeledStatement.GetStatement());

            foreach (var nodeStatement in a)
            foreach (var statement in b)
                if (statement != nodeStatement)
                    graph.AddDirectedEdge(nodeStatement, statement, 1);
        }


        private static List<GraphNode<SStatement>> FindNodes(Graph<SStatement> graph, SStatement statement1)
        {
            var hashSet = new HashSet<GraphNode<SStatement>>();
            foreach (var graphNode in graph.Nodes)
                if (statement1 != null && graphNode.Value == statement1.GetStatement())
                    hashSet.Add((GraphNode<SStatement>) graphNode);
            return hashSet.ToList();
        }
    }
}