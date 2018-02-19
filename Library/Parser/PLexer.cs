using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Library.Parser
{
    public static class PLexer
    {
        private static readonly Dictionary<string, PTokenType> TokensList =
            new Dictionary<string, PTokenType>
            {
                {"auto", PTokenType.Auto},
                {"break", PTokenType.Break},
                {"case", PTokenType.Case},
                {"char", PTokenType.Char},
                {"const", PTokenType.Const},
                {"continue", PTokenType.Continue},
                {"default", PTokenType.Default},
                {"do", PTokenType.Do},
                {"double", PTokenType.Double},
                {"else", PTokenType.Else},
                {"enum", PTokenType.Enum},
                {"extern", PTokenType.Extern},
                {"float", PTokenType.Float},
                {"for", PTokenType.For},
                {"goto", PTokenType.Goto},
                {"if", PTokenType.If},
                {"int", PTokenType.Int},
                {"long", PTokenType.Long},
                {"register", PTokenType.Register},
                {"return", PTokenType.Return},
                {"short", PTokenType.Short},
                {"signed", PTokenType.Signed},
                {"sizeof", PTokenType.Sizeof},
                {"static", PTokenType.Static},
                {"struct", PTokenType.Struct},
                {"switch", PTokenType.Switch},
                {"typedef", PTokenType.Typedef},
                {"union", PTokenType.Union},
                {"unsigned", PTokenType.Unsigned},
                {"void", PTokenType.Void},
                {"volatile", PTokenType.Volatile},
                {"while", PTokenType.While},

                {"[a-zA-Z_]([a-zA-Z_]|\\d)*", PTokenType.Identifier},

                {"0[xX][a-fA-F0-9]+(u|U|l|L)*?", PTokenType.Constant},
                {"0\\d+(u|U|l|L)*?", PTokenType.Constant},
                {"\\d+(u|U|l|L)*?", PTokenType.Constant},
                {"L?'(\\.|[^\\'])+'", PTokenType.Constant},

                {"\\d+[Ee][+-]?\\d+(f|F|l|L)?", PTokenType.Constant},
                {"\\d*\\.\\d+([Ee][+-]?\\d+)?(f|F|l|L)?", PTokenType.Constant},
                {"\\d+\\.\\d*([Ee][+-]?\\d+)?(f|F|l|L)?", PTokenType.Constant},

                {"L?\"(\\.|[^\\\"])*\"", PTokenType.StringLiteral},

                {"...", PTokenType.Ellipsis},
                {">>=", PTokenType.RightAssign},
                {"<<=", PTokenType.LeftAssign},
                {"+=", PTokenType.AddAssign},
                {"-=", PTokenType.SubAssign},
                {"*=", PTokenType.MulAssign},
                {"/=", PTokenType.DivAssign},
                {"%=", PTokenType.ModAssign},
                {"&=", PTokenType.AndAssign},
                {"^=", PTokenType.XorAssign},
                {"|=", PTokenType.OrAssign},
                {">>", PTokenType.RightOp},
                {"<<", PTokenType.LeftOp},
                {"++", PTokenType.IncOp},
                {"--", PTokenType.DecOp},
                {"->", PTokenType.PtrOp},
                {"&&", PTokenType.AndOp},
                {"||", PTokenType.OrOp},
                {"<=", PTokenType.LeOp},
                {">=", PTokenType.GeOp},
                {"==", PTokenType.EqOp},
                {"!=", PTokenType.NeOp},

                {";", PTokenType.Semicolon},
                {"{", PTokenType.LeftBrace},
                {"<%", PTokenType.LeftBrace},
                {"}", PTokenType.RightBrace},
                {"%>", PTokenType.RightBrace},
                {",", PTokenType.Comma},
                {":", PTokenType.Colon},
                {"=", PTokenType.AssignOp},
                {"(", PTokenType.LeftParen},
                {")", PTokenType.RightParen},
                {"[", PTokenType.LeftBracket},
                {"<:", PTokenType.LeftBracket},
                {"]", PTokenType.RightBracket},
                {":>", PTokenType.RightBracket},
                {".", PTokenType.InstOp},
                {"&", PTokenType.BitAndOp},
                {"!", PTokenType.NotOp},
                {"~", PTokenType.BitNotOp},
                {"-", PTokenType.SubOp},
                {"+", PTokenType.AddOp},
                {"*", PTokenType.MulOp},
                {"/", PTokenType.DivOp},
                {"%", PTokenType.ModOp},
                {"<", PTokenType.LtOp},
                {">", PTokenType.GtOp},
                {"^", PTokenType.XorOp},
                {"|", PTokenType.BitOrOp},
                {"?", PTokenType.Query},

                {"/*", PTokenType.Comment},
                {"//", PTokenType.Comment},
                {"#", PTokenType.Comment} /* Preprocessor directives interpreted as comments! */
            };

        private static readonly char[] Whitespace = {' ', '\t', '\v', '\r', '\n', '\f'};

        public static PTokenType GetTokenType(string tokenCode)
        {
            return TokensList[tokenCode];
        }

        public static PToken[] ParseTokens(string code)
        {
            int currLineIndex = 0, currCharIndex = 0;
            var tokenCollection = new List<PToken>();

            code = JoinLines(code);

            while (currCharIndex < code.Length)
                tokenCollection.Add(MatchToken(code, ref currLineIndex, ref currCharIndex));

            return tokenCollection.ToArray();
        }

        public static bool TokenExists(string tokenCode)
        {
            return TokensList.ContainsKey(tokenCode);
        }

        private static bool GetToken(string code, string token, int currCharIndex, ref string resultString)
        {
            var substrdCode = code.Substring(currCharIndex);

            if (IsRegex(token) &&
                Regex.IsMatch(substrdCode, token) &&
                Regex.Match(substrdCode, token).Index == 0)
            {
                resultString = Regex.Match(substrdCode, token).Value;
                return true;
            }

            if (substrdCode.Length >= token.Length &&
                token == code.Substring(currCharIndex, token.Length))
            {
                resultString = token;
                return true;
            }

            return false;
        }

        private static bool IsRegex(string key)
        {
            return TokensList[key] == PTokenType.Identifier ||
                   TokensList[key] == PTokenType.Constant ||
                   TokensList[key] == PTokenType.StringLiteral;
        }

        private static bool IsWhitespace(char c)
        {
            return Whitespace.Any(space => c == space);
        }

        private static string JoinLines(string originalCode)
        {
            return originalCode.Replace("\\\r\n", "").Replace("\\\n", "");
        }

        private static PToken MatchToken(string code, ref int currLineIndex, ref int currCharIndex)
        {
            var maxLength = 0;
            PToken currToken = null;
            var currTokenCode = "";

            for (; currCharIndex < code.Length && IsWhitespace(code[currCharIndex]); currCharIndex++)
                if (code[currCharIndex] == '\n')
                    currLineIndex++;

            if (currCharIndex == code.Length)
                return new PToken("", PTokenType.Eof, currLineIndex, currCharIndex);

            foreach (var token in TokensList.Keys)
                if (GetToken(code, token, currCharIndex, ref currTokenCode))
                    if (currTokenCode.Length > maxLength)
                    {
                        maxLength = currTokenCode.Length;
                        currToken = new PToken(currTokenCode, TokensList[token], currLineIndex, currCharIndex);
                    }

            if (maxLength == 0)
                throw new InvalidOperationException("Invalid identifier!");

            currCharIndex += maxLength;

            if (currToken.TokenCode == "/*")
            {
                for (;
                    currCharIndex < code.Length - 1 && code[currCharIndex] != '*' || code[currCharIndex + 1] != '/';
                    currCharIndex++)
                    if (code[currCharIndex] == '\n')
                        currLineIndex++;

                if (currCharIndex == code.Length - 1)
                    throw new InvalidOperationException("No terminating */ comment!");

                currCharIndex += 2;

                return MatchToken(code, ref currLineIndex, ref currCharIndex);
            }

            if (currToken.TokenCode == "//" || currToken.TokenCode == "#")
            {
                for (; currCharIndex < code.Length && code[currCharIndex] != '\n'; currCharIndex++) ;
                currLineIndex++;

                return MatchToken(code, ref currLineIndex, ref currCharIndex);
            }

            return currToken;
        }
    }
}