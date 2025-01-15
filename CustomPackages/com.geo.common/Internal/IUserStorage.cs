namespace Geo.Common.Public
{
    public interface IUserStorage
    {
        int Index { get; }
        int Coins { get; }
        void AddCoins(int coins);
        void SetIndex(int value);
    }
}
