using BallMaze.GameManagement;
using BallMaze.GameMechanics.Balls;
using BallMaze.GameMechanics.Tiles;
using System;
using UnityEngine;

namespace BallMaze.GameMechanics
{
    public class CubeView : MonoBehaviour
    {
        public const int NB_FACES = 6;

        protected IBallController[,,] interior;
        protected TileController[][,] exterior = new TileController[6][,];

        private CubeData model;

        public float TileSize
        {
            get
            {
                return SizeElements.SIZE_TILE_X * SizeRatio;
            }
        }
        private const float BASE_BOARD_SIZE = 3;

        public virtual float SizeRatio
        {
            get
            {
                return BASE_BOARD_SIZE / Mathf.Max(model.X_SIZE, Math.Max(model.Y_SIZE, model.Z_SIZE));
            }
        }


        void Start()
        {
            RefreshView(CubeData.GetDummyCubeData());
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                CameraTurnAround cameraController = GameObject.FindGameObjectWithTag(Tags.CameraController).GetComponent<CameraTurnAround>();
                Level3DManager loader = GameObject.FindGameObjectWithTag(Tags.BallMazeController).GetComponent<Level3DManager>();
                loader.CreateSlice(model.GetBoardAtFace(cameraController.GetCurrentFace()));
            }
        }

        internal Vector3 GetBallPosition(int posX, int posY, int posZ)
        {
            return new Vector3(posX - (float)(model.X_SIZE - 1) / 2, posY - (float)(model.Y_SIZE - 1) / 2, posZ - (float)(model.Z_SIZE - 1) / 2) * TileSize;
        }

        internal Vector3 GetTilePosition(int faceNumber, int sideIndex, int upIndex)
        {
            float width;
            float height;
            switch (faceNumber)
            {
                case CubeFace.X:
                case CubeFace.MX:
                    width = model.Z_SIZE;
                    height = model.Y_SIZE;
                    break;
                case CubeFace.Y:
                case CubeFace.MY:
                    width = model.X_SIZE;
                    height = model.Z_SIZE;
                    break;
                case CubeFace.Z:
                case CubeFace.MZ:
                    width = model.X_SIZE;
                    height = model.Y_SIZE;
                    break;
                default:
                    throw new UnhandledSwitchCaseException(faceNumber);
            }
            return new Vector3(sideIndex - (width - 1) / 2, 0, upIndex - (height - 1) / 2) * TileSize;
        }

        internal Vector3 GetFacePosition(int faceNumber)
        {
            Vector3 result;
            switch (faceNumber)
            {
                case CubeFace.X:
                    result = Vector3.left * (float)model.X_SIZE / 2;
                    break;
                case CubeFace.MX:
                    result = Vector3.right * (float)model.X_SIZE / 2;
                    break;
                case CubeFace.Y:
                    result = Vector3.down * (float)model.Y_SIZE / 2;
                    break;
                case CubeFace.MY:
                    result = Vector3.up * (float)model.Y_SIZE / 2;
                    break;
                case CubeFace.Z:
                    result = Vector3.back * (float)model.Z_SIZE / 2;
                    break;
                case CubeFace.MZ:
                    result = Vector3.forward * (float)model.Z_SIZE / 2;
                    break;
                default:
                    throw new UnhandledSwitchCaseException(faceNumber);
            }
            return result * TileSize;
        }

        internal Quaternion GetFaceRotation(int faceNumber)
        {
            switch (faceNumber)
            {
                case CubeFace.X:
                    return Quaternion.Euler(0, 0, -90);
                case CubeFace.MX:
                    return Quaternion.Euler(0, 0, 90);
                case CubeFace.Y:
                    return Quaternion.Euler(0, 0, 0);
                case CubeFace.MY:
                    return Quaternion.Euler(0, 0, 180);
                case CubeFace.Z:
                    return Quaternion.Euler(90, 0, 0);
                case CubeFace.MZ:
                    return Quaternion.Euler(-90, 0, 0);
                default:
                    throw new UnhandledSwitchCaseException(faceNumber);
            }
        }

        public void RefreshView(CubeData model)
        {
            this.model = model;
            interior = new IBallController[model.X_SIZE, model.Y_SIZE, model.Z_SIZE];
            CreateBalls(model);
            CreateTiles(model);
        }

        private void CreateTiles(CubeData data)
        {
            int widthFace;
            int heightFace;
            for (int faceNumber = 0; faceNumber < 6; faceNumber++)
            {
                GameObject face = new GameObject(faceNumber + "");
                face.transform.SetParent(transform);
                face.transform.localRotation = GetFaceRotation(faceNumber);
                face.transform.localPosition = GetFacePosition(faceNumber);
                switch (faceNumber)
                {
                    case CubeFace.X:
                    case CubeFace.MX:
                        widthFace = data.Z_SIZE;
                        heightFace = data.Y_SIZE;
                        break;
                    case CubeFace.Y:
                    case CubeFace.MY:
                        widthFace = data.X_SIZE;
                        heightFace = data.Z_SIZE;
                        break;
                    case CubeFace.Z:
                    case CubeFace.MZ:
                        widthFace = data.X_SIZE;
                        heightFace = data.Y_SIZE;
                        break;
                    default:
                        throw new UnhandledSwitchCaseException(faceNumber);
                }
                exterior[faceNumber] = new TileController[widthFace, heightFace];
                for (int x = 0; x < widthFace; x++)
                {
                    for (int y = 0; y < heightFace; y++)
                    {
                        exterior[faceNumber][x, y] = TileCreator.CreateTile(data.tiles[faceNumber, x, y], GetTilePosition(faceNumber, x, y), 1);
                        exterior[faceNumber][x, y].transform.SetParent(face.transform, false);
                    }
                }
            }
        }

        private void CreateBalls(CubeData data)
        {
            for (int x = 0; x < data.X_SIZE; x++)
            {
                for (int y = 0; y < data.Y_SIZE; y++)
                {
                    for (int z = 0; z < data.Z_SIZE; z++)
                    {
                        IBallController ball = BallCreator.GetBall(data.balls[x, y, z], 1);
                        ball.Init(x, y, z,this);

                        interior[x, y, z] = ball;
                    }
                }
            }
        }
    }
}
