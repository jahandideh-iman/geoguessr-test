using System;

namespace GeoGuessr.Game
{
    public class Player
    {
        public int Score { get; private set; }
        public Player(int index)
        {
            Index = index;
            Score = 0;
        }

        public int Index { get; }

        public void AddScore(int value)
        {
            Score += value;
        }
    }
}