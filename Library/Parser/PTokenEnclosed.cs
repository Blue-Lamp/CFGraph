namespace Library.Parser
{
    internal class PTokenEnclosed : IMatchable
    {
        private readonly PToken _tokenEnd;
        private readonly PToken _tokenStart;

        public PTokenEnclosed(PToken tokenStart, PToken tokenEnd)
        {
            _tokenStart = tokenStart;
            _tokenEnd = tokenEnd;
        }

        public MatchResult match(PToken[] tokens, int startIndex)
        {
            int i, countStartTokens = 0, countEndTokens = 0;

            if (tokens[startIndex].TokenType != _tokenStart.TokenType)
                return new MatchResult(0);

            for (i = startIndex; i < tokens.Length; i++)
            {
                if (tokens[i].TokenType == _tokenStart.TokenType)
                    countStartTokens++;
                else if (tokens[i].TokenType == _tokenEnd.TokenType)
                    countEndTokens++;

                if (countStartTokens == countEndTokens)
                    return new MatchResult(i - startIndex + 1);
            }

            return new MatchResult(0);
        }
    }
}