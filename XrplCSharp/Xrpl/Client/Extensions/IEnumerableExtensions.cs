using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Xrpl.ClientLib.Extensions
{
    public static class EnumerableExtensions
    {
        /// <summary>Check collection if contains element</summary>
        /// <typeparam name="T">element type</typeparam>
        /// <param name="collection">current collection</param>
        /// <param name="selector">function to check collection</param>
        /// <returns>true, if predicate one of of collection element is true</returns>
        [DebuggerStepThrough]
        public static bool Contains<T>(this IEnumerable<T> collection, Func<T, bool> selector) =>
            collection switch
            {
                T[] objArray => objArray.Any(selector),
                List<T> objList1 => objList1.Any(selector),
                IList<T> objList2 => objList2.Any(selector),
                _ => collection.Any(selector)
            };
    }
}
