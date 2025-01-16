using UnityEngine;

namespace Geo.Common.Public
{
    public sealed class UserStorage : IUserStorage
    {
        private const string INDEX_KEY = "Index";
        private const string COINS_KEY = "Coins";

        public int Index { 
            get => PlayerPrefs.GetInt(INDEX_KEY, 0); 
            private set => PlayerPrefs.SetInt(INDEX_KEY, value);
        }

        public int Coins {
            get => PlayerPrefs.GetInt(COINS_KEY, 0);
            private set => PlayerPrefs.SetInt(COINS_KEY, value);
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
