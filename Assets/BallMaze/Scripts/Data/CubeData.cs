using BallMaze.Data;
using BallMaze.Exceptions;
using BallMaze.GameMechanics.LevelCreation;
using BallMaze.LevelCreation;
using System;
using System.Xml.Serialization;

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

    public BoardData GetBoardAtFace(int faceNumber)
    {
        BoardData data = new BoardData();
        data.tiles = tiles.Get(faceNumber);
        switch (faceNumber)
        {
            case CubeFace.X:
                data = GetBoardData(data, Z_SIZE, false, Y_SIZE, false, X_SIZE, true, (model, f, s, t) => model.balls[t, s, f]);
                break;
            case CubeFace.MX:
                data = GetBoardData(data, Z_SIZE, true, Y_SIZE, false, X_SIZE, false, (model, f, s, t) => model.balls[t, s, f]);
                break;
            case CubeFace.Y:
                data = GetBoardData(data, X_SIZE, false, Z_SIZE, false, Y_SIZE, true, (model, f, s, t) => model.balls[f, t, s]);
                break;
            case CubeFace.MY:
                data = GetBoardData(data, X_SIZE, false, Z_SIZE, true, Y_SIZE, false, (model, f, s, t) => model.balls[f, t, s]);
                break;
            case CubeFace.Z:
                data = GetBoardData(data, X_SIZE, true, Y_SIZE, false, Z_SIZE, true, (model, f, s, t) => model.balls[f, s, t]);
                break;
            case CubeFace.MZ:
                data = GetBoardData(data, X_SIZE, false, Y_SIZE, false, Z_SIZE, false, (model, f, s, t) => model.balls[f, s, t]);
                break;
            default:
                throw new UnhandledSwitchCaseException(faceNumber);
        }
        return data;
    }

    private BoardData GetBoardData(BoardData data, int sizeFirst, bool inverseFirst, int sizeSecond, bool inverseSecond, int sizeThird, bool inverseThird, Func<CubeData, int, int, int, BallData> GetBall)
    {
        data.balls = new BallData[sizeFirst, sizeSecond];
        for (int first = 0; first < sizeFirst; first++)
        {
            for (int second = 0; second < sizeSecond; second++)
            {
                int third = 0;
                int iFirst = Inverse(first, sizeFirst, inverseFirst);
                int iSecond = Inverse(second, sizeSecond, inverseSecond);
                while (third < sizeThird)
                {
                    int iThird = Inverse(third, sizeThird, inverseThird);

                    BallData ball = GetBall(this,iFirst, iSecond, iThird);
                    if (ball.BallType != BallType.EMPTY)
                    {
                        data.balls[first, second] = ball;
                        data.tiles[first, second] = TileData.GetNormalTile();
                        break;
                    }
                    third++;
                }
                if (third >= sizeThird)
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
