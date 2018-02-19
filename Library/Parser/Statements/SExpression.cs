namespace Library.Parser.Statements
{
    public sealed class SExpression
    {
        public readonly string CodeString;

        public SExpression(string codeString)
        {
            CodeString = codeString;
        }

        public string GetDisplayValue()
        {
            return CodeString.Trim();
        }

        public override string ToString()
        {
            return "/* Expression: */ " + CodeString;
        }
    }
}