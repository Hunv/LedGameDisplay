using System;
using System.Collections.Generic;
using System.Text;

namespace LedGameDisplayLibrary.Effects
{
    public class Counter : IEffect
    {
        public int TeamId { get; set; } = -1;

        public override void Execute()
        {
            Console.WriteLine("TeamId {0} score is {1}", TeamId, GameManager.Teams[TeamId].Score);
            Display.ShowString(GameManager.Teams[TeamId].Score.ToString(), GameManager.Teams[TeamId].ScoreArea);
            Display.Render();
        }

        public void Increase()
        {
            GameManager.Teams[TeamId].Score++;
            Execute();
        }

        public void Decrease()
        {
            GameManager.Teams[TeamId].Score--;
            Execute();
        }

        public void Reset()
        {
            GameManager.Teams[TeamId].Score = 0;
            Execute();
        }
    }
}
