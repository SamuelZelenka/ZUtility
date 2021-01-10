using System;
using System.Collections;
using System.Collections.Generic;

namespace ZUtility
{
    public static class ListExtension
    {
        /// <summary>
        /// Return a random item from this list.
        /// </summary>
        public static T GetRandomItem<T>(this List<T> list)
        {
            return list[new Random().Next(0, list.Count)];
        }
    }
}
