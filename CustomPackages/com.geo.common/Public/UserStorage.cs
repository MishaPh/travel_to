using UnityEngine;

namespace Geo.Common.Public
{
    public sealed class UserStorage : IUserStorage
    {
        public int Index { 
            get => PlayerPrefs.GetInt("Index", 0); 
            private set => PlayerPrefs.SetInt("Index", value);
        }

        public int Coins {
            get => PlayerPrefs.GetInt("Score", 0);
            private set => PlayerPrefs.SetInt("Score", value);
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
