using System;
using System.Collections;
using System.Collections.Generic;

namespace Library.Parser
{
    public sealed class PMatchedData : IEnumerable<PToken>
    {
        private readonly PToken[] _matched;
        public readonly int NumMatches;
        public readonly int StartIndex;

        public PMatchedData(PToken[] tokens, int startIndex, int numMatches)
        {
            StartIndex = startIndex;
            _matched = new PToken[numMatches];
            Array.Copy(tokens, startIndex, _matched, 0, numMatches);
            NumMatches = numMatches;
        }

        public PToken this[int i] => _matched[i];

        public IEnumerator<PToken> GetEnumerator()
        {
            return ((IEnumerable<PToken>) _matched).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _matched.GetEnumerator();
        }

        public PToken[] GetTokens()
        {
            return (PToken[]) _matched.Clone();
        }
    }
}