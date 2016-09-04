using UnityEngine;

namespace Utilities
{
    static class ArrayExtensions
    {
        public static string Print<Type>(this Type[] value)
        {
            if (value == null)
            {
                return "The array is null " + value.ToString();
            }
            if (value.Length < 1)
            {
                return "The array is empty " + value.ToString();
            }
            string result = "(";
            result += value[0];
            for (int i = 1; i < value.Length; i++)
            {
                result += "," + value[i];
            }
            result += ")";
            return result;
        }

        public static double Average(this double[] array)
        {
            double sum = 0;
            for (int i = 0; i < array.Length; i++)
            {
                sum += array[i];
            }
            return sum / array.Length;
        }

        public static double[] GetMinValues(this double[][] array)
        {
            int size = array[0].Length;
            double[] result = new double[size];
            for (int i = 0; i < size; i++)
            {
                result[i] = array[0][i];
                for (int y = 0; y < array.Length; y++)
                {
                    if (array[y][i] < result[i])
                    {
                        result[i] = array[y][i];
                    }
                }
            }
            return result;
        }

        public static void PrintAllToConsole(this double[][][] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Debug.Log("------------------");
                for (int y = 0; y < array[i].Length; y++)
                {
                    Debug.Log(array[i][y].Print());
                }
            }

        }
    }
}
