public static class MatrixExtensions
{


    public static T[][][] ToJaggedArray<T>(this T[,,] matrix)
    {
        T[][][] jaggedArray = new T[matrix.GetLength(0)][][];
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            jaggedArray[i] = new T[matrix.GetLength(1)][];

            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                jaggedArray[i][j] = new T[matrix.GetLength(2)];
                for (int k = 0; k < matrix.GetLength(1); k++)
                {
                    jaggedArray[i][j][k] = matrix[i, j, k];
                }
            }
        }
        return jaggedArray;
    }

    public static T[,,] ToMatrix<T>(this T[][][] jaggedArray)
    {
        if (jaggedArray.Length > 0 && jaggedArray[0].Length > 0)
        {
            T[,,] matrix = new T[jaggedArray.Length, jaggedArray[0].Length, jaggedArray[0][0].Length];
            for (int i = 0; i < jaggedArray.Length; i++)
            {
                for (int j = 0; j < jaggedArray[0].Length; j++)
                    for (int k = 0; k < jaggedArray[0].Length; k++)
                        matrix[i, j, k] = jaggedArray[i][j][k];
            }
            return matrix;
        }
        else return new T[0, 0, 0];
    }


    public static T[][] ToJaggedArray<T>(this T[,] matrix)
    {
        T[][] jaggedArray = new T[matrix.GetLength(0)][];
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            jaggedArray[i] = new T[matrix.GetLength(1)];

            for (int j = 0; j < matrix.GetLength(1); j++)
                jaggedArray[i][j] = matrix[i, j];
        }
        return jaggedArray;
    }

    public static T[,] ToMatrix<T>(this T[][] jaggedArray)
    {
        if (jaggedArray.Length > 0)
        {
            T[,] matrix = new T[jaggedArray.Length, jaggedArray[0].Length];
            for (int i = 0; i < jaggedArray.Length; i++)
            {
                for (int j = 0; j < jaggedArray[0].Length; j++)
                    matrix[i, j] = jaggedArray[i][j];
            }
            return matrix;
        }
        else return new T[0, 0];
    }
}
