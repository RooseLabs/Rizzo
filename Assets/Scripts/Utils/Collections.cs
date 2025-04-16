using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace RooseLabs.Utils
{
    public static class Collections
    {
        /// <summary>
        ///   <para>Shuffles the elements of the list in place using the Fisher-Yates algorithm.</para>
        /// </summary>
        public static void Shuffle<T>(this IList<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
