using System;
using System.Collections.Generic;
using System.Text;

namespace YetAnotherTextRpg.Helpers
{
    public static class DiceHelper
    {
        private static Random _rnd = new Random();

        public static int RollD6()
        {
            return _rnd.Next(1, 7);
        }
    }
}
