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
                    listBox1.Items.Clear();
                    var graph = new Graph<SStatement>();
                    matchedFunction.BuildGraphNodes(graph);
                    listBox1.Items.Clear();
                    listBox2.Items.Clear();
                    listBox3.Items.Clear();

                    foreach (var token in tokens)
                        listBox1.Items.Add(token.LineNumber + "_" + token.StartSym + "\t<" + token.TokenType + "> " +
                                           token.TokenCode);

                    for (int i = 0, j = 0; i < graph.Nodes.Count; i++)
                    {
                        var node = graph.Nodes[i];
                        if (node.Value.Visible)
                            listBox3.Items.Add(++j + ": " + node.Value.CodeString);
                        listBox2.Items.Add(i + ": " + node.Value.CodeString);
                    }

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