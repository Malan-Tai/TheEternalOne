using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheEternalOne.Code.Utils
{
    public static class Dice
    {
        static Random rand = new Random();
        public static int GetRandint(int min, int max)
        {
            return rand.Next(min, max);
        }
    }
}
