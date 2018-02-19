namespace Library.Parser
{
    public interface IMatchable
    {
        MatchResult match(PToken[] tokens, int startIndex);
    }
}