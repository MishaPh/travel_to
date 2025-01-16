using UnityEngine;

namespace Geo.Common.Public
{
    public sealed class UserStorage : IUserStorage
    {
        private const string IndexKey = "Index";
        private const string CoinsKey = "Coins";

        public int Index { 
            get => PlayerPrefs.GetInt(IndexKey, 0); 
            private set => PlayerPrefs.SetInt(IndexKey, value);
        }

        public int Coins {
            get => PlayerPrefs.GetInt(CoinsKey, 0);
            private set => PlayerPrefs.SetInt(CoinsKey, value);
        }

        public void AddCoins(int score)
        {
           Coins += score;
        }

        public void SetIndex(int value)
        {
            Index = value;
        }
    }
}
