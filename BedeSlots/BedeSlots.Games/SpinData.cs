using System.Collections.Generic;

namespace BedeSlots.Games
{
    public class SpinData
    {
        public string[,] Matrix { get; set; }

        public decimal Amount { get; set; }

        public List<int> WinningRows { get; set; }

        public double Coefficient { get; set; }
    }
}
