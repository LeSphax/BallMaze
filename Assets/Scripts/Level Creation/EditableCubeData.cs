


public class EditableCubeData : CubeData
{
    private const int XSize_MAX = 10;
    private const int YSize_MAX = 10;

    private bool initialised;
    internal CubeController cubeController;

    private TileData[][,] oldFaces = new TileData[6][,];
    private BallData[,,] oldBalls = new BallData[0, 0, 0];

    public IntVector3 Sizes
    {
        get
        {
            return new IntVector3(balls.GetLength(0), balls.GetLength(1), balls.GetLength(2));
        }
    }

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
            return balls.GetLength(1);
        }
    }
    public EditableCubeData()
    {
    }

    internal EditableCubeData(CubeController controller)
    {
        cubeController = controller;
        for (int i = 0; i < FaceModel.NUMBER_FACES; i++)
        {
            oldFaces[i] = new TileData[0, 0];
        }
    }

    public void DestroyBoard()
    {
        SaveBoard();
        ResetBoard();
    }

    private void SaveBoard()
    {
        oldFaces = faces;
        oldBalls = balls;
    }

    public void ResetBoard()
    {
        oldFaces = TileData.CreateEmptyFaceArray(new IntVector3(0, 0, 0));
        oldBalls = BallData.CreateEmptyBallDataMatrix(new IntVector3(0, 0, 0));
        CreateBoard(IntVector3.One *3);
        UpdateModel();
    }

    public void CreateBoard(IntVector3 sizes)
    {
        faces = TileData.CreateEmptyFaceArray(sizes, oldFaces);
        balls = InitBalls(sizes);
        UpdateModel();
    }

    public void ChangeFaceSize(CubeFace face, int XSize, int YSize)
    {
        FaceModel faceModel = FaceModel.ModelsDictionary[face];
        IntVector3 sizes = faceModel.GetRealSizes(XSize, YSize, balls.GetLength((int)faceModel.axes[2]));
        CreateBoard(sizes);
    }

    private BallData[,,] InitBalls(IntVector3 sizes)
    {
        BallData[,,] result = new BallData[sizes.X, sizes.Y, sizes.Z];
        for (int i = 0; i < sizes.X; i++)
        {
            for (int j = 0; j < sizes.Y; j++)
            {
                for (int k = 0; k < sizes.Z; k++)
                {
                    if (oldBalls.GetLength(0) > i && oldBalls.GetLength(1) > j && oldBalls.GetLength(2) > k)
                    {
                        result[i, j, k] = oldBalls[i, j, k];
                    }
                    else
                    {
                        result[i, j, k] = BallData.GetEmptyBall();
                    }
                }
            }
        }
        return result;
    }

    public void NextBall(IntVector3 position)
    {
        balls.Set(position, balls.Get(position).GetNext());
        UpdateModel();
    }

    internal void PreviousBall(IntVector3 position)
    {
        balls.Set(position, balls.Get(position).GetPrevious());
        UpdateModel();
    }

    public void NextTileObjective(int face, int posX, int posY)
    {
        if (!TileExists(face, posX, posY))
            faces[face][posX, posY].ObjectiveType = faces[face][posX, posY].ObjectiveType.Next();
        UpdateModel();
    }

    public void NextTileType(int face, int posX, int posY)
    {
        if (!TileExists(face, posX, posY))
            faces[face][posX, posY].TileType = faces[face][posX, posY].TileType.Next();
        UpdateModel();
    }

    public void PreviousTileObjective(int face, int posX, int posY)
    {
        if (!TileExists(face, posX, posY))
            faces[face][posX, posY].ObjectiveType = faces[face][posX, posY].ObjectiveType.Previous();
        UpdateModel();
    }

    public void PreviousTileType(int face, int posX, int posY)
    {
        if (!TileExists(face, posX, posY))
            faces[face][posX, posY].TileType = faces[face][posX, posY].TileType.Previous();
        UpdateModel();
    }

    internal void SetData(CubeData cubeData)
    {
        faces = cubeData.faces;
        balls = cubeData.balls;
        UpdateModel();
    }

    public bool TileExists(int face, int posX, int posY)
    {
        if (faces[face][posX, posY] == null)
        {
            return true;
        }
        return false;
    }

    private void UpdateModel()
    {
        cubeController.SetData(this, !initialised);
        if (!initialised)
        {
            initialised = true;
        }
    }
}
