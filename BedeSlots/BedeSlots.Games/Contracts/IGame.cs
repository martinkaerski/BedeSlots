using System.Collections.Generic;

namespace BedeSlots.Games.Contracts
{
    public interface IGame
    {
        Item[,] GenerateMatrix(int rows, int cols, IDictionary<int, Item> items);

        string[,] GetCharMatrix(Item[,] matrix);

        SpinData Spin(int rows, int cols, decimal money);

        string[,] GenerateCharMatrix(int rows, int cols);
    }
}