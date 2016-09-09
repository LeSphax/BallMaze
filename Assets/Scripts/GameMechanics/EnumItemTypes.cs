using BallMaze.Cube;
using BallMaze.GameMechanics;
using System;
using UnityEngine;
using UnityEngine.Assertions;

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

    public static BallData[,,] GetEmptyBallDataMatrix(int width, int height, int depth)
    {
        BallData[,,] matrix = new BallData[width, height, depth];
        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
                for (int k = 0; k < height; k++)
                    matrix[i, j, k] = GetEmptyBall();
        return matrix;
    }

    internal static BallData GetNormalBall()
    {
        return new BallData(BallType.NORMAL);
    }

    internal static BallData GetObjective1Ball()
    {
        return new BallData(BallType.NORMAL, ObjectiveType.OBJECTIVE1);
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

    public override int GetHashCode()
    {
        return BallType.GetHashCode() + ObjectiveType.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        if (obj.GetType() == typeof(BallData))
        {
            BallData other = (BallData)obj;
            if (other.BallType == BallType && other.ObjectiveType == ObjectiveType)
            {
                return true;
            }
        }
        return false;
    }

    public override string ToString()
    {
        return "BallData : " + BallType + "    " + ObjectiveType;
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

    public static TileData[][,] GetEmptyFaceArray(IntVector3 sizes, TileData[][,] oldFaces = null)
    {
        TileData[][,] result = new TileData[FaceModel.NUMBER_FACES][,];
        for (int i = 0; i < FaceModel.NUMBER_FACES; i++)
        {
            FaceModel face = FaceModel.ModelsDictionary[(CubeFace)i];
            int XSize = sizes[face.axes[0]];
            int YSize = sizes[face.axes[1]];
            result[i] = new TileData[XSize, YSize];

            for (int x = 0; x < XSize; x++)
            {
                for (int y = 0; y < YSize; y++)
                {
                    if (oldFaces != null && oldFaces[i].GetLength(0) > x && oldFaces[i].GetLength(1) > y)
                    {
                        result[i][x, y] = oldFaces[i][x, y];
                    }
                    else
                        result[i][x, y] = GetNormalTile();
                }
            }
        }
        return result;
    }

    internal static TileData GetObjective1Tile()
    {
        return new TileData(ObjectiveType.OBJECTIVE1, TileType.NORMAL);
    }

    internal static TileData GetObjective2Tile()
    {
        return new TileData(ObjectiveType.OBJECTIVE2, TileType.NORMAL);
    }
}


