using BallMaze.Data;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CubeModel : MonoBehaviour
{
    public BallData[,,] balls;
    public TileData[,,] faces;

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
            return faces.ToJaggedArray();
        }
        set
        {
            faces = value.ToMatrix();
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
        return CheckObjectives();
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
        foreach (TileData tile in faces)
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

    public void SetDummyCubeModel()
    {
        balls = BallData.GetEmptyBallDataMatrix(3, 3, 3);
        faces = TileData.GetEmptyTileDataMatrix(6, 3, 3);
        balls[0, 0, 0] = BallData.GetObjective1Ball();
        balls[1, 1, 1] = BallData.GetWall();
        balls[0, 1, 0] = BallData.GetWall();
        balls[0, 1, 1] = BallData.GetWall();
        balls[0, 0, 2] = BallData.GetWall();
        balls[2, 1, 2] = BallData.GetWall();
        //
        faces[5, 0, 2] = TileData.GetObjective1Tile();
        //faces[0, 2, 2] = TileData.GetObjective2Tile();
        //faces[1, 1, 2] = TileData.GetObjective1Tile();
        //faces[1, 2, 1] = TileData.GetObjective2Tile();
        //faces[2, 1, 0] = TileData.GetObjective1Tile();
        //faces[2, 2, 0] = TileData.GetObjective2Tile();
        //faces[3, 0, 2] = TileData.GetObjective1Tile();
        //faces[3, 2, 1] = TileData.GetObjective2Tile();
        //faces[4, 2, 0] = TileData.GetObjective2Tile();
        //faces[5, 0, 1] = TileData.GetObjective1Tile();
        //faces[5, 0, 2] = TileData.GetObjective2Tile();
    }

    public BoardData GetBoardAtFace(Vector3 rotation)
    {
        BoardData data = new BoardData();
        CubeFace face = CameraTurnAround.GetFace(rotation);
        TileData[,] faceTiles = faces.Get((int)face);
        BallData[,] ballData = GetPlaneBoardData(face);
        int faceRotation = GetFaceRotation(face, rotation);

        data.balls = ballData.Rotate(faceRotation);
        data.tiles = faceTiles.Rotate(faceRotation);
        Assert.IsNotNull(data.balls);
        return data;
    }

    private static int GetFaceRotation(CubeFace face, Vector3 rotation)
    {
        switch (face)
        {
            case CubeFace.X:
            case CubeFace.MX:
            case CubeFace.Z:
            case CubeFace.MZ:
                return Mathf.RoundToInt(rotation.z);
            case CubeFace.Y:
                return Mathf.RoundToInt(360 - rotation.y);
            case CubeFace.MY:
                return Mathf.RoundToInt(rotation.y);
            default:
                throw new UnhandledSwitchCaseException(face);
        }
    }

    private BallData[,] GetPlaneBoardData(CubeFace face)
    {
        int[] sizes;
        bool inverse = false;
        int mirrorAxis = -1;
        Func<BallData[,,], int, int, int, BallData> getter;
        switch (face)
        {
            case CubeFace.X:
            case CubeFace.MX:
                sizes = new int[3] { Z_SIZE, Y_SIZE, X_SIZE };
                getter = (model, z, y, x) => model[x, y, z];
                break;
            case CubeFace.Y:
            case CubeFace.MY:
                sizes = new int[3] { X_SIZE, Z_SIZE, Y_SIZE };
                getter = (model, x, z, y) => model[x, y, z];
                break;
            case CubeFace.Z:
            case CubeFace.MZ:
                sizes = new int[3] { Y_SIZE, X_SIZE, Z_SIZE };
                getter = (model, x, y, z) => model[x, y, z];
                break;
            default:
                throw new UnhandledSwitchCaseException(face);
        }
        switch (face)
        {
            case CubeFace.X:
            case CubeFace.Y:
            case CubeFace.Z:
                inverse = true;
                break;
            case CubeFace.MX:
            case CubeFace.MZ:
            case CubeFace.MY:
                inverse = false;
                break;
        }
        switch (face)
        {
            case CubeFace.MZ:
            case CubeFace.X:
            case CubeFace.Y:
                break;
            case CubeFace.Z:
            case CubeFace.MX:
                mirrorAxis = 0;
                break;
            case CubeFace.MY:
                mirrorAxis = 1;
                break;
        }
        if (mirrorAxis == -1)
            return GetPlaneBoardData(balls, sizes[0], sizes[1], sizes[2], inverse, getter);
        else
            return GetPlaneBoardData(balls, sizes[0], sizes[1], sizes[2], inverse, getter).Mirror(mirrorAxis);
    }

    private BallData[,] GetPlaneBoardData(BallData[,,] model, int xSize, int ySize, int zSize, bool inverseZ, Func<BallData[,,], int, int, int, BallData> getter)
    {
        BallData[,] result = new BallData[xSize, ySize];
        for (int first = 0; first < xSize; first++)
        {
            for (int second = 0; second < ySize; second++)
            {
                int third = 0;
                while (third < zSize)
                {
                    int iThird = Functions.Inverse(third, zSize, inverseZ);

                    BallData ball = getter(model, first, second, iThird);
                    if (ball.BallType != BallType.EMPTY)
                    {
                        result[first, second] = ball;
                        break;
                    }
                    third++;
                }
                if (third >= zSize)
                {
                    result[first, second] = BallData.GetEmptyBall();
                }
            }
        }
        return result;
    }

    public void SetNewBallPositions(Dictionary<BallData, Coords> newPositions)
    {
        Dictionary<BallData, Coords> currentPositions = new Dictionary<BallData, Coords>();
        for (int x = 0; x < X_SIZE; x++)
            for (int y = 0; y < Y_SIZE; y++)
                for (int z = 0; z < Z_SIZE; z++)
                    if (balls[x, y, z].BallType == BallType.NORMAL)
                        currentPositions.Add(balls[x, y, z], new Coords(x, y, z));

        foreach (var pair in newPositions)
        {
            balls[pair.Value.x, pair.Value.y] =
        }
    }
}
