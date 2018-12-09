using System.Collections.Generic;
using System.Linq;

namespace BedeSlots.Games
{
    public static class GenerateItems
    {
        public static IDictionary<int, Item> GetItems()
        {
            var apple = new Item
            {
                Name = "a",
                Coefficient = 0.4m,
                Probability = 45,
                Type = ItemType.Normal
            };

            var banana = new Item
            {
                Name = "b",
                Coefficient = 0.6m,
                Probability = 35,
                Type = ItemType.Normal
            };

            var pineapple = new Item
            {
                Name = "p",
                Coefficient = 0.8m,
                Probability = 15,
                Type = ItemType.Normal
            };

            var wildcard = new Item
            {
                Name = "w",
                Coefficient = 0m,
                Probability = 5,
                Type = ItemType.Wildcard

            };

            var items = new List<Item>
            {
                apple,
                banana,
                pineapple,
                wildcard
            };

            var sortedItems = items.OrderBy(i => i.Probability).ToList();

            int previousCumulativeProb = 0;
            for (int i = 0; i < sortedItems.Count; i++)
            {
                var currentCumulativeProb = sortedItems[i].Probability + previousCumulativeProb;
                sortedItems[i].CumulativeProbability = currentCumulativeProb;
                previousCumulativeProb = currentCumulativeProb;
            }

            var sortedItemsDict = new SortedDictionary<int, Item>
            {
                { apple.CumulativeProbability, apple },
                { pineapple.CumulativeProbability, pineapple },
                { banana.CumulativeProbability, banana },
                { wildcard.CumulativeProbability, wildcard }
            };

            return sortedItemsDict;
        }
    }
}
