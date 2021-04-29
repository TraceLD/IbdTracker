using System;

namespace IbdTracker.Core.Extensions
{
    public static class ArrayExtensions
    {
        public static T GetRandomElement<T>(this T[] arr)
        {
            var random = new Random();
            return arr[random.Next(arr.Length)];
        }
    }
}