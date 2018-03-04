using System;
using System.Windows.Forms;
using Library.Graph;
using Library.Parser;
using Library.Parser.Statements;

namespace CFGraphFormApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            button1_Click(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DisplayProgram(textBox1.Text);
        }

        private void DisplayProgram(string program)
        {
            try
            {
                var tokens = PLexer.ParseTokens(program);

                foreach (var matchedFunction in PParser.GetFunctionContents(tokens))
                {
                   
                    var graph = new Graph<SStatement>();
                    matchedFunction.BuildGraphNodes(graph);
                   
                    RemoveUnvisible(graph);
                    var matrix = graph.GetMatrix();

                    dataGridView1.ColumnCount = matrix.GetLength(0);
                    dataGridView1.RowCount = matrix.GetLength(1);
                    dataGridView1.ColumnHeadersHeight = 4;
                    dataGridView1.RowHeadersWidth = 4;

                    for (var i0 = 0; i0 < matrix.GetLength(0); i0++)
                    {
                        dataGridView1.Columns[i0].HeaderText = (i0 + 1).ToString();
                        dataGridView1.Columns[i0].Width = 17;
                        for (var i1 = 0; i1 < matrix.GetLength(1); i1++)
                            dataGridView1[i1, i0].Value = matrix[i0, i1];
                    }
                }
            }
            catch (ArgumentException exc)
            {
                MessageBox.Show(@"There is an error in source code: " + exc.Message);
            }
            catch (InvalidOperationException exc)
            {
                MessageBox.Show(@"There is an error in source code: " + exc.Message);
            }
            catch
            {
                MessageBox.Show(@"An unexpected error occured!");
            }
        }

        private void RemoveUnvisible(Graph<SStatement> graph)
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