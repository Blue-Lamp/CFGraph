using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.Parser
{
    public class PToken : IMatchable
    {
        public static readonly PTokenAlternative AnyToken;
        public static readonly PToken EmptyToken = new PEmptyToken();
        public readonly int LineNumber;
        public readonly int StartSym;
        public readonly string TokenCode;
        public readonly PTokenType TokenType;

        static PToken()
        {
            var allTokens = new List<PToken>();

            foreach (PTokenType tokenType in Enum.GetValues(typeof(PTokenType)))
                allTokens.Add(new PToken(tokenType));

            AnyToken = new PTokenAlternative(allTokens.ToArray());
        }

        public PToken(string tokenCode, int lineIndex = -1, int startSymIndex = -1)
        {
            TokenCode = tokenCode;

            if (PLexer.TokenExists(tokenCode))
                TokenType = PLexer.GetTokenType(tokenCode);

            LineNumber = lineIndex;
            StartSym = startSymIndex;
        }

        public PToken(string tokenCode, PTokenType tokenType, int lineIndex = -1, int startSymIndex = -1)
        {
            TokenCode = tokenCode;

            TokenType = tokenType;

            LineNumber = lineIndex + 1;
            StartSym = startSymIndex + 1;
        }

        public PToken(PTokenType tokenType, int lineIndex = -1, int startSymIndex = -1)
        {
            TokenType = tokenType;

            LineNumber = lineIndex + 1;
            StartSym = startSymIndex + 1;
        }

        public MatchResult match(PToken[] tokens, int startIndex)
        {
            return new MatchResult(startIndex < tokens.Length && TokenType == tokens[startIndex].TokenType ? 1 : 0);
        }

        public static PTokenAlternative AnyTokenBesides(params PTokenType[] exceptions)
        {
            var allTokens = new List<PToken>();

            foreach (PTokenType tokenType in Enum.GetValues(typeof(PTokenType)))
                if (!Enumerable.Contains(exceptions, tokenType))
                    allTokens.Add(new PToken(tokenType));

            return new PTokenAlternative(allTokens.ToArray());
        }

        public override string ToString()
        {
            return TokenCode;
        }
    }

    public sealed class PEmptyToken : PToken, IMatchable
    {
        public PEmptyToken() : base("")
        {
        }

        public new MatchResult match(PToken[] tokens, int startIndex)
        {
            return new MatchResult(0, true, true, null);
        }
    }
}