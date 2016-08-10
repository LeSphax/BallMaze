using BallMaze.Data;
using BallMaze.LevelCreation;
using System;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Assertions;

[XmlInclude(typeof(EditableBoardData))]
public class CubeData
{
    [XmlIgnore]
    public BallData[,,] balls;
    [XmlIgnore]
    public TileData[,,] tiles;

    public int X_SIZE
    {
        get
        {
            return balls.GetLength(0);
        }
    }
    public int Y_SIZE
    {
        get
        {
            return balls.GetLength(1);
        }
    }
    public int Z_SIZE
    {
        get
        {
            return balls.GetLength(2);
        }
    }

    public TileData[][][] serializedTiles
    {
        get
        {
            return tiles.ToJaggedArray();
        }
        set
        {
            tiles = value.ToMatrix();
        }
    }

    public BallData[][][] serializedBalls
    {
        get
        {
            return balls.ToJaggedArray();
        }
        set
        {
            balls = value.ToMatrix();
        }
    }

    public bool IsValid()
    {
        if (balls.GetLength(0) != tiles.GetLength(0) || balls.GetLength(1) != tiles.GetLength(1) || balls.GetLength(2) != tiles.GetLength(2))
            return false;
        else
        {
            return CheckObjectives();
        }
    }

    private bool CheckObjectives()
    {
        int numberObjectiveTypes = 2;
        int[] ObjectiveTiles = new int[numberObjectiveTypes];
        int[] ObjectiveBalls = new int[numberObjectiveTypes];

        foreach (BallData ball in balls)
        {
            if (ball.ObjectiveType == ObjectiveType.OBJECTIVE1)
            {
                ObjectiveBalls[0] += 1;
            }
            else if (ball.ObjectiveType == ObjectiveType.OBJECTIVE2)
            {
                ObjectiveBalls[1] += 1;
            }
        }
        foreach (TileData tile in tiles)
        {

            if (tile.ObjectiveType == ObjectiveType.OBJECTIVE1)
            {
                ObjectiveTiles[0] += 1;
            }
            else if (tile.ObjectiveType == ObjectiveType.OBJECTIVE2)
            {
                ObjectiveTiles[1] += 1;
            }

        }

        for (int i = 0; i < numberObjectiveTypes; i++)
        {
            if (ObjectiveBalls[i] != ObjectiveTiles[i])
            {
                return false;
            }
        }
        return true;
    }

    public static CubeData GetDummyCubeData()
    {
        CubeData board = new CubeData();
        board.balls = BallData.GetEmptyBallDataMatrix(3, 3, 3);
        board.tiles = TileData.GetEmptyTileDataMatrix(6, 3, 3);
        board.balls[0, 0, 0] = BallData.GetObjective1Ball();
        board.balls[1, 1, 1] = BallData.GetWall();
        board.balls[0, 1, 0] = BallData.GetWall();
        board.balls[0, 0, 2] = BallData.GetWall();
        board.tiles[1, 2, 2] = TileData.GetObjective1Tile();
        return board;
    }

    public BoardData GetBoardAtFace(Vector3 rotation)
    {
        BoardData data = new BoardData();
        CubeFace face = CameraTurnAround3.GetFace(rotation);
        TileData[,] faceTiles = tiles.Get((int)face);
        switch (face)
        {
            case CubeFace.X:
                if (Mathf.RoundToInt(rotation.z) % 360 == 0)
                    data = GetBoardData(data, faceTiles,Z_SIZE, false, Y_SIZE, false, X_SIZE, true, (model, z, y, x) => model.balls[x, y, z]);
                else if (Mathf.RoundToInt(rotation.z) == 90)
                    data = GetBoardData(data, faceTiles, Y_SIZE, false, Z_SIZE, true, X_SIZE, true, (model, y, z, x) => model.balls[x, y, z]);
                else if (Mathf.RoundToInt(rotation.z) == 180)
                    data = GetBoardData(data, faceTiles, Z_SIZE, true, Y_SIZE, true, X_SIZE, true, (model, z, y, x) => model.balls[x, y, z]);
                else if (Mathf.RoundToInt(rotation.z) == 270)
                    data = GetBoardData(data, faceTiles, Y_SIZE, true, Z_SIZE, false, X_SIZE, true, (model, y, z, x) => model.balls[x, y, z]);
                break;
            case CubeFace.MX:
                if (Mathf.RoundToInt(rotation.z) % 360 == 0)
                    data = GetBoardData(data, faceTiles, Z_SIZE, true, Y_SIZE, false, X_SIZE, false, (model, z, y, x) => model.balls[x, y, z]);
                else if (Mathf.RoundToInt(rotation.z) == 90)
                    data = GetBoardData(data, faceTiles, Y_SIZE, false, Z_SIZE, false, X_SIZE, false, (model, y, z, x) => model.balls[x, y, z]);
                else if (Mathf.RoundToInt(rotation.z) == 180)
                    data = GetBoardData(data, faceTiles, Z_SIZE, false, Y_SIZE, true, X_SIZE, false, (model, z, y, x) => model.balls[x, y, z]);
                else if (Mathf.RoundToInt(rotation.z) == 270)
                    data = GetBoardData(data, faceTiles, Y_SIZE, true, Z_SIZE, true, X_SIZE, false, (model, y, z, x) => model.balls[x, y, z]);
                break;
            case CubeFace.Y:
                if (Mathf.RoundToInt(rotation.y) % 360 == 0)
                    data = GetBoardData(data, faceTiles, X_SIZE, false, Z_SIZE, false, Y_SIZE, true, (model, x, z, y) => model.balls[x, y, z]);
                else if (Mathf.RoundToInt(rotation.y) == 90)
                    data = GetBoardData(data, faceTiles, Z_SIZE, false, X_SIZE, false, Y_SIZE, true, (model, z, x, y) => model.balls[x, y, z]);
                else if (Mathf.RoundToInt(rotation.y) == 180)
                    data = GetBoardData(data, faceTiles, X_SIZE, true, Z_SIZE, true, Y_SIZE, true, (model, x, z, y) => model.balls[x, y, z]);
                else if (Mathf.RoundToInt(rotation.y) == 270)
                    data = GetBoardData(data, faceTiles, Z_SIZE, true, X_SIZE, true, Y_SIZE, true, (model, z, x, y) => model.balls[x, y, z]);
                break;
            case CubeFace.MY:
                if (Mathf.RoundToInt(rotation.y) % 360 == 0)
                    data = GetBoardData(data, faceTiles, X_SIZE, false, Z_SIZE, true, Y_SIZE, false, (model, x, z, y) => model.balls[x, y, z]);
                else if (Mathf.RoundToInt(rotation.y) == 90)
                    data = GetBoardData(data, faceTiles, Z_SIZE, true, X_SIZE, true, Y_SIZE, false, (model, z, x, y) => model.balls[x, y, z]);
                else if (Mathf.RoundToInt(rotation.y) == 180)
                    data = GetBoardData(data, faceTiles, X_SIZE, true, Z_SIZE, false, Y_SIZE, false, (model, x, z, y) => model.balls[x, y, z]);
                else if (Mathf.RoundToInt(rotation.y) == 270)
                    data = GetBoardData(data, faceTiles, Z_SIZE, false, X_SIZE, false, Y_SIZE, false, (model, z, x, y) => model.balls[x, y, z]);
                break;
            case CubeFace.Z:
                if (Mathf.RoundToInt(rotation.z) % 360 == 0)
                    data = GetBoardData(data, faceTiles, X_SIZE, true, Y_SIZE, false, Z_SIZE, true, (model, x, y, z) => model.balls[x, y, z]);
                else if (Mathf.RoundToInt(rotation.z) == 90)
                    data = GetBoardData(data, faceTiles, Y_SIZE, false, X_SIZE, false, Z_SIZE, true, (model, y, x, z) => model.balls[x, y, z]);
                else if (Mathf.RoundToInt(rotation.z) == 180)
                    data = GetBoardData(data, faceTiles, X_SIZE, false, Y_SIZE, true, Z_SIZE, true, (model, x, y, z) => model.balls[x, y, z]);
                else if (Mathf.RoundToInt(rotation.z) == 270)
                    data = GetBoardData(data, faceTiles, Y_SIZE, true, X_SIZE, true, Z_SIZE, true, (model, y, x, z) => model.balls[x, y, z]);

                break;
            case CubeFace.MZ:
                if (Mathf.RoundToInt(rotation.z) % 360 == 0)
                    data = GetBoardData(data, faceTiles, X_SIZE, false, Y_SIZE, false, Z_SIZE, false, (model, x, y, z) => model.balls[x, y, z]);
                else if (Mathf.RoundToInt(rotation.z) == 90)
                    data = GetBoardData(data, faceTiles, Y_SIZE, false, X_SIZE, true, Z_SIZE, false, (model, y, x, z) => model.balls[x, y, z]);
                else if (Mathf.RoundToInt(rotation.z) == 180)
                    data = GetBoardData(data, faceTiles, X_SIZE, true, Y_SIZE, true, Z_SIZE, false, (model, x, y, z) => model.balls[x, y, z]);
                else if (Mathf.RoundToInt(rotation.z) == 270)
                    data = GetBoardData(data, faceTiles, Y_SIZE, true, X_SIZE, false, Z_SIZE, false, (model, y, x, z) => model.balls[x, y, z]);

                break;
            default:
                throw new UnhandledSwitchCaseException(face);
        }
        Assert.IsNotNull(data.balls);
        return data;
    }

    private BoardData GetBoardData(BoardData data, TileData[,] tiles, int xSize, bool inverseX, int ySize, bool inverseY, int zSize, bool inverseZ, Func<CubeData, int, int, int, BallData> GetBall)
    {
        data.balls = new BallData[xSize, ySize];
        data.tiles = new TileData[xSize, ySize];
        for (int first = 0; first < xSize; first++)
        {
            for (int second = 0; second < ySize; second++)
            {
                int third = 0;
                int iFirst = Inverse(first, xSize, inverseX);
                int iSecond = Inverse(second, ySize, inverseY);
                data.tiles[first, second] = tiles[iFirst, iSecond];
                while (third < zSize)
                {
                    int iThird = Inverse(third, zSize, inverseZ);

                    BallData ball = GetBall(this, iFirst, iSecond, iThird);
                    if (ball.BallType != BallType.EMPTY)
                    {
                        DebugExtensions.Log(iFirst, iSecond, third);
                        data.balls[first, second] = ball;
                        data.tiles[first, second] = TileData.GetNormalTile();
                        break;
                    }
                    third++;
                }
                if (third >= zSize)
                {
                    data.balls[first, second] = BallData.GetEmptyBall();
                }
            }
        }
        return data;
    }

    private static int Inverse(int value, int size, bool inverse)
    {
        if (inverse)
        {
            value = size - 1 - value;
        }
        return value;
    }
}
