using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheBatmanGame.Extensions
{
    static class CollectionExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            foreach (var item in collection)
            {
                action(item);
            }
        }
        public static void ForEach<T>(this IEnumerable<T> collection, Action<int, T> action)
        {
            int index = 0;
            foreach (var item in collection)
            {
                action(index, item);
                index++;
            }
        }

    }
}
