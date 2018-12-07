using BedeSlots.Games.Contracts;
using System;
using System.Collections.Generic;

namespace BedeSlots.Games
{
    public class Game : IGame
    {
        private IDictionary<int, Item> items;

        public Game()
        {
            items = GenerateItems.GetItems();
        }

        public SpinData Spin(int rows, int cols, decimal money)
        {
            var matrix = GenerateMatrix(rows, cols, items);
            var coefficient = CalculateCoefficient(matrix);

            if (coefficient != 0)
            {
                money = (decimal)coefficient * money;
            }
            else
            {
                money = 0;
            }

            var spinData = new SpinData()
            {
                Matrix = GetCharMatrix(matrix),
                Money = money
            };

            return spinData;
        }

        private double CalculateCoefficient(Item[,] matrix)
        {
            double finalCoef = 0;

            for (int row = 0; row < matrix.GetLength(0); row++)
            {
                Item previousItem = null;
                bool isWinning = true;
                double rowCoef = 0;

                for (int col = 0; col < matrix.GetLength(1); col++)
                {
                    //can be optimised - element = GenerateItem()
                    var element = matrix[row, col];

                    if (element.Type == ItemType.Wildcard)
                    {
                        continue;
                    }

                    if (previousItem != null)
                    {
                        if (element != previousItem)
                        {
                            isWinning = false;
                            break;
                        }
                        else
                        {
                            rowCoef += element.Coefficient;
                            previousItem = element;
                        }
                    }
                    else
                    {
                        rowCoef += element.Coefficient;
                        previousItem = element;
                    }
                }

                if (isWinning)
                {
                    finalCoef += rowCoef;
                }
            }

            return finalCoef;
        }

        public Item[,] GenerateMatrix(int rows, int cols, IDictionary<int, Item> items)
        {
            var matrix = new Item[rows, cols];
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    matrix[row, col] = GetRandomItem(items);
                }
            }

            return matrix;
        }

        public string[,] GetCharMatrix(Item[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            string[,] stringMatrix = new string[rows, cols];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    stringMatrix[row, col] = matrix[row, col].Name;
                }
            }

            return stringMatrix;
        }

        public string[,] GenerateCharMatrix(int rows, int cols)
        {
            string[,] stringMatrix = new string[rows, cols];

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    stringMatrix[row, col] = GetRandomItem(items).Name;
                }
            }

            return stringMatrix;
        }

        // key - cumulative probability
        private Item GetRandomItem(IDictionary<int, Item> items)
        {
            var random = new Random();
            var randomNumber = random.Next(1, 101); //the maxValue is exclusive

            Item selectedItem = null;
            foreach (var item in items)
            {
                //randomNumber is betwwen 1 and 100
                if (randomNumber <= item.Key)
                {
                    selectedItem = item.Value;
                    break;
                }
            }

            return selectedItem;
        }
    }
}
