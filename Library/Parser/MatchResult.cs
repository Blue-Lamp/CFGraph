namespace Library.Parser
{
    public sealed class MatchResult : IMatchable
    {
        public readonly Matcher GetNextMatch;
        public readonly bool IsMatch;
        public readonly bool MatchComplete;
        public readonly int NumMatches;

        public MatchResult(int numMatches, bool matchComplete = true, Matcher getNextMatch = null)
        {
            NumMatches = numMatches;
            IsMatch = numMatches > 0;
            MatchComplete = matchComplete;
            GetNextMatch = getNextMatch;
        }

        public MatchResult(int numMatches, bool isMatch, bool matchComplete, Matcher getNextMatch)
        {
            NumMatches = numMatches;
            IsMatch = isMatch;
            MatchComplete = matchComplete;
            GetNextMatch = getNextMatch;
        }

        public MatchResult match(PToken[] tokens, int startIndex)
        {
            return !MatchComplete ? GetNextMatch(tokens, startIndex) : this;
        }
    }
}