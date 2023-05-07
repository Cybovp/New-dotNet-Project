using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Extensions
{
    public static class EnumerableExtensions
    {
        public static void MyForEach<T>( this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source.ToList())
                action(item);
        }
    }
}
