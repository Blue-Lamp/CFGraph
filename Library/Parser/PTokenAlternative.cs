using System;

namespace Library.Parser
{
    public class PTokenAlternative : IMatchable
    {
        private readonly IMatchable[] _contents;

        public PTokenAlternative(params IMatchable[] containedTokens)
        {
            _contents = new IMatchable[containedTokens.Length];
            Array.Copy(containedTokens, _contents, containedTokens.Length);
        }

        public MatchResult match(PToken[] tokens, int startIndex)
        {
            var currResult = MatchNext(tokens, startIndex, (IMatchable[]) _contents.Clone(), 0);

            while (!currResult.IsMatch && !currResult.MatchComplete)
                currResult = currResult.match(tokens, startIndex);

            return currResult;
        }

        private MatchResult MatchNext(PToken[] tokens, int startIndex, IMatchable[] matchers, int matchersStartIndex)
        {
            var currResult = matchers[matchersStartIndex].match(tokens, startIndex);
            Matcher getNextMatch = null;
            var matchComplete = false;

            matchers[matchersStartIndex] = currResult;

            if (currResult.MatchComplete && matchersStartIndex < _contents.Length - 1)
                getNextMatch = (tokenCollection, tokenStartIndex) =>
                    MatchNext(tokenCollection, tokenStartIndex, (IMatchable[]) matchers.Clone(),
                        matchersStartIndex + 1);
            else if (!currResult.MatchComplete)
                getNextMatch = (tokenCollection, tokenStartIndex) =>
                    MatchNext(tokenCollection, tokenStartIndex, (IMatchable[]) matchers.Clone(), matchersStartIndex);
            else if (currResult.MatchComplete && matchersStartIndex == _contents.Length - 1)
                matchComplete = true;

            return new MatchResult(currResult.NumMatches, matchComplete, getNextMatch);
        }
    }
}