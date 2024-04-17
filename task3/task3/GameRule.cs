using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace task3
{
    public class GameRule
    {
        public string GetWinner(string[] args, int userMove, int computerMove)
        {
            int n = args.Length;
            int range = n / 2;

            if (userMove == computerMove)
            {
                return "Draw";
            }
            else if (userMove > range)
            {
                int distance = userMove - range;
                if (computerMove < distance || computerMove > userMove)
                {
                    return "You Win!";
                }
                else
                {
                    return "You Lose!";
                }
            }
            else if ((userMove + 1) % n <= computerMove && computerMove <= (userMove + range) % n ||
                     (userMove + 1 - n) <= computerMove && computerMove <= (userMove + range - n))
            {
                return "You Win!";
            }
            else
            {
                return "You Lose!";
            }
        }
    }
}
