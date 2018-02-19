namespace Library.Parser
{
    public class PTokenSequence : IMatchable
    {
        private readonly IMatchable[] _contents;

        public PTokenSequence(params IMatchable[] containedTokens)
        {
            _contents = (IMatchable[]) containedTokens.Clone();
        }

        public MatchResult match(PToken[] tokens, int startIndex)
        {
            return matchNext(tokens, startIndex, (IMatchable[]) _contents.Clone(), 0);
        }

        private MatchResult matchNext(PToken[] tokens, int startIndex, IMatchable[] matchersSequence,
            int matchersSequenceStartIndex)
        {
            MatchResult currResult;

            do
            {
                matchersSequence[matchersSequenceStartIndex] =
                    currResult = matchersSequence[matchersSequenceStartIndex].match(tokens, startIndex);

                MatchResult nextResult = null;

                if (currResult.IsMatch && matchersSequenceStartIndex < matchersSequence.Length - 1)
                    do
                    {
                        nextResult = nextResult == null
                            ? matchNext(tokens, startIndex + currResult.NumMatches,
                                (IMatchable[]) matchersSequence.Clone(), matchersSequenceStartIndex + 1)
                            : nextResult.match(tokens, startIndex + currResult.NumMatches);

                        if (!nextResult.IsMatch) continue;
                        if (!nextResult.MatchComplete)
                            return new MatchResult(currResult.NumMatches + nextResult.NumMatches, false,
                                (tokensArr, tokensStartIndex) =>
                                    matchNext(tokensArr, tokensStartIndex,
                                        (IMatchable[]) matchersSequence.Clone(), matchersSequenceStartIndex + 1));
                        if (!currResult.MatchComplete)
                            return new MatchResult(currResult.NumMatches + nextResult.NumMatches, false,
                                (tokensArr, tokensStartIndex) =>
                                    matchNext(tokensArr, tokensStartIndex,
                                        (IMatchable[]) matchersSequence.Clone(), matchersSequenceStartIndex));
                        if (nextResult.MatchComplete)
                            return new MatchResult(currResult.NumMatches + nextResult.NumMatches);
                    } while (!nextResult.MatchComplete);
                else if (matchersSequenceStartIndex == matchersSequence.Length - 1)
                    return currResult;
            } while (!currResult.MatchComplete);

            return new MatchResult(0);
        }
    }
}