using System;
using System.Linq;
using Library.Graph;
using Library.Parser;
using Library.Parser.Statements;

namespace CFGraphUnitTest.Helper
{
    internal static class Helper
    {
        public static bool Compare(int[,] input, int[,] output)
        {
            if (input.GetLength(0) != output.GetLength(0) || input.GetLength(1) != output.GetLength(1))
                return false;

            for (var y = 0; y < input.GetLength(0); y++)
            for (var x = 0; x < input.GetLength(1); x++)
                if (input[y, x] != output[y, x])
                    return false;
            return true;
        }

        public static int[,] GetMatrixFromCode(string code)
        {
            var graph = new Graph<SStatement>();
            var tokens = PLexer.ParseTokens(code);
            PParser.GetFunctionContents(tokens).First().BuildGraphNodes(graph);

            RemoveUnvisible(graph);
            return graph.GetMatrix();
        }

        public static string MatrixToString(int[,] ints)
        {
            var result = "";
            for (var j = 0; j < ints.GetLength(0); j++)
            {
                for (var k = 0; k < ints.GetLength(1); k++)
                    result = result + (ints[j, k] + " ");
                result += Environment.NewLine;
            }

            return result;
        }

        private static void RemoveUnvisible(Graph<SStatement> graph)
        {
            for (var i = 0; i < graph.Nodes.Count; i++)
                if (graph.Nodes[i].Value.Visible == false)
                {
                    graph.RemoveByValueAndAddEdges(graph.Nodes[i].Value);
                    i--;
                }
        }
    }
}