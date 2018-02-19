using System;
using System.Linq;

namespace Library.Parser
{
    internal class PTokenRepeat : IMatchable
    {
        public delegate bool MatchCompleteChecker(PToken[] tokens, int startIndex, int numMatched);

        private readonly IMatchable _content;
        private readonly bool _greedyMatch;
        private readonly bool _infiniteRepeat;
        private readonly MatchCompleteChecker _matchCompleteChecker;
        private readonly int _repeatLowerBound;

        private PTokenRepeat(IMatchable content, bool greedyMatch, int repeatLowerBound, int repeatUpperBound,
            MatchCompleteChecker matchCompleteChecker = null)
        {
            _content = content;
            _repeatLowerBound = repeatLowerBound;
            RepeatUpperBound = repeatUpperBound;
            _greedyMatch = greedyMatch;
            _matchCompleteChecker = matchCompleteChecker;
        }

        public PTokenRepeat(IMatchable content, bool greedyMatch, int repeatLowerBound = 0,
            MatchCompleteChecker matchCompleteChecker = null)
            : this(content, greedyMatch, repeatLowerBound, 0, matchCompleteChecker)
        {
            _infiniteRepeat = true;
        }

        private int RepeatUpperBound { get; set; }

        public MatchResult match(PToken[] tokens, int startIndex)
        {
            var numRepeat = _greedyMatch
                ? (_infiniteRepeat ? tokens.Length - startIndex : RepeatUpperBound)
                : _repeatLowerBound;

            if (_infiniteRepeat)
                RepeatUpperBound = tokens.Length - startIndex;

            var currResult = MatchNext(tokens, startIndex, GetRepeatSequence(numRepeat), numRepeat);

            while ((!currResult.IsMatch && _matchCompleteChecker == null ||
                    _matchCompleteChecker != null && !_matchCompleteChecker(tokens, startIndex, currResult.NumMatches))
                   && !currResult.MatchComplete)
                currResult = currResult.match(tokens, startIndex);

            return currResult;
        }


        private PTokenSequence GetRepeatSequence(int numRepeat)
        {
            if (numRepeat == 0)
                return new PTokenSequence(PToken.EmptyToken);

            if (numRepeat > 0)
                return new PTokenSequence(
                    Enumerable.ToArray(Enumerable.Repeat(_content, numRepeat))
                );

            throw new ArgumentOutOfRangeException(nameof(numRepeat), "numRepeat should be non-negative!");
        }

        private MatchResult MatchNext(PToken[] tokens, int startIndex, IMatchable currSequence, int numRepeat)
        {
            var currResult = currSequence.match(tokens, startIndex);
            var nextNumRepeat = _greedyMatch ? numRepeat - 1 : numRepeat + 1;

            if (!currResult.MatchComplete)
                return new MatchResult(currResult.NumMatches, false,
                    (tokensArr, tokensStartIndex) => MatchNext(tokensArr, tokensStartIndex, currResult, numRepeat));

            if (nextNumRepeat < _repeatLowerBound || nextNumRepeat > RepeatUpperBound)
                return currResult;

            return new MatchResult(currResult.NumMatches, currResult.IsMatch, false,
                (tokensArr, tokensStartIndex) => MatchNext(tokensArr, tokensStartIndex,
                    GetRepeatSequence(nextNumRepeat), nextNumRepeat));
        }
    }
}