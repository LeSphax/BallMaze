using System;

[Serializable]
public enum ObjectiveType
{
    NONE,
    OBJECTIVE1,
    OBJECTIVE2,
}


[Serializable]
public enum BallType
{
    EMPTY,
    WALL,
    NORMAL,
}

[Serializable]
public enum TileType
{
    NORMAL,
    SYNCED,
}

public static class MyExtensions
{
    public static Enum Next<Enum>(this Enum src) where Enum : struct
    {
        if (!typeof(Enum).IsEnum) throw new ArgumentException(String.Format("Argumnent {0} is not an Enum", typeof(Enum).FullName));

        Enum[] Arr = (Enum[])System.Enum.GetValues(src.GetType());
        int j = Array.IndexOf<Enum>(Arr, src) + 1;
        return (Arr.Length == j) ? Arr[0] : Arr[j];
    }

    public static Enum Previous<Enum>(this Enum src) where Enum : struct
    {
        if (!typeof(Enum).IsEnum) throw new ArgumentException(String.Format("Argumnent {0} is not an Enum", typeof(Enum).FullName));

        Enum[] Arr = (Enum[])System.Enum.GetValues(src.GetType());
        int j = Array.IndexOf<Enum>(Arr, src) -1;
        return (0 > j) ? Arr[Arr.Length-1] : Arr[j];
    }
}
[Serializable]
public class BallData
{
    public BallType BallType { get; set; }
    public ObjectiveType ObjectiveType { get; set; }


    public BallData()
    {
    }

    public BallData(BallType ball, ObjectiveType objective = ObjectiveType.NONE)
    {
        BallType = ball;
        ObjectiveType = objective;
    }

    public BallData GetNext()
    {
        switch (BallType)
        {
            case BallType.NORMAL:
                switch (ObjectiveType)
                {
                    case ObjectiveType.OBJECTIVE2:
                        return GetEmptyBall();
                    default:
                        return new BallData(BallType, ObjectiveType.Next());
                }
            default:
                return new BallData(BallType.Next());
        }
    }

    public static BallData GetEmptyBall()
    {
        return new BallData(BallType.EMPTY);
    }

    public static BallData[,] GetEmptyBallDataMatrix(int width, int height)
    {
        BallData[,] matrix = new BallData[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
                matrix[i, j] = GetEmptyBall();
        }
        return matrix;
    }

    internal static BallData GetNormalBall()
    {
        return new BallData(BallType.NORMAL);
    }

    internal static BallData GetObjective1Ball()
    {
        return new BallData(BallType.NORMAL,ObjectiveType.OBJECTIVE1);
    }

    internal static BallData GetObjective2Ball()
    {
        return new BallData(BallType.NORMAL, ObjectiveType.OBJECTIVE2);
    }

    public static BallData GetWall()
    {
        return new BallData(BallType.WALL);
    }

    public BallData GetPrevious()
    {
        switch (BallType)
        {
            case BallType.NORMAL:
                switch (ObjectiveType)
                {
                    case ObjectiveType.NONE:
                        return GetWall();
                    default:
                        return new BallData(BallType, ObjectiveType.Previous());
                }
            case BallType.EMPTY:
                return GetObjective2Ball();
            default:
                return new BallData(BallType.Previous());
        }
    }
}

[Serializable]
public class TileData
{
    public ObjectiveType ObjectiveType { get; set; }
    public TileType TileType { get; set; }

    public TileData()
    {

    }

    public TileData(ObjectiveType objective, TileType tile)
    {
        ObjectiveType = objective;
        TileType = tile;
    }

    public static TileData GetNormalTile()
    {
        return new TileData(ObjectiveType.NONE, TileType.NORMAL);
    }

    public static TileData[,] GetEmptyTileDataMatrix(int width, int height)
    {
        TileData[,] matrix = new TileData[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
                matrix[i, j] = GetNormalTile();
        }
        return matrix;
    }

    internal static TileData GetObjective1Tile()
    {
        return new TileData(ObjectiveType.OBJECTIVE1, TileType.NORMAL);
    }
}


