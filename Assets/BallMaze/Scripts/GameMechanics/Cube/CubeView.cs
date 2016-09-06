using BallMaze.Data;
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

        private CubeModel currentModel;

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
                return BASE_BOARD_SIZE / Mathf.Max(currentModel.X_SIZE, Math.Max(currentModel.Y_SIZE, currentModel.Z_SIZE));
            }
        }

        internal Vector3 GetBallPosition(int posX, int posY, int posZ)
        {
            return new Vector3(posX - (float)(currentModel.X_SIZE - 1) / 2, posY - (float)(currentModel.Y_SIZE - 1) / 2, posZ - (float)(currentModel.Z_SIZE - 1) / 2) * TileSize;
        }

        internal Vector3 GetTilePosition(CubeFace faceNumber, int sideIndex, int upIndex)
        {
            float width;
            float height;

            switch (faceNumber)
            {
                case CubeFace.X:
                case CubeFace.MX:
                    width = currentModel.Z_SIZE;
                    height = currentModel.Y_SIZE;
                    break;
                case CubeFace.Y:
                case CubeFace.MY:
                    width = currentModel.X_SIZE;
                    height = currentModel.Z_SIZE;
                    break;
                case CubeFace.Z:
                case CubeFace.MZ:
                    width = currentModel.X_SIZE;
                    height = currentModel.Y_SIZE;
                    break;
                default:
                    throw new UnhandledSwitchCaseException(faceNumber);
            }
            return new Vector3(sideIndex - (width - 1) / 2, 0, upIndex - (height - 1) / 2) * TileSize;
        }

        internal Vector3 GetFacePosition(CubeFace faceNumber)
        {
            Vector3 result;
            switch (faceNumber)
            {
                case CubeFace.X:
                    result = Vector3.left * (float)currentModel.X_SIZE / 2;
                    break;
                case CubeFace.MX:
                    result = Vector3.right * (float)currentModel.X_SIZE / 2;
                    break;
                case CubeFace.Y:
                    result = Vector3.down * (float)currentModel.Y_SIZE / 2;
                    break;
                case CubeFace.MY:
                    result = Vector3.up * (float)currentModel.Y_SIZE / 2;
                    break;
                case CubeFace.Z:
                    result = Vector3.back * (float)currentModel.Z_SIZE / 2;
                    break;
                case CubeFace.MZ:
                    result = Vector3.forward * (float)currentModel.Z_SIZE / 2;
                    break;
                default:
                    throw new UnhandledSwitchCaseException(faceNumber);
            }
            return result * TileSize;
        }

        internal Quaternion GetFaceRotation(CubeFace faceNumber)
        {
            switch (faceNumber)
            {
                case CubeFace.X:
                    return Quaternion.Euler(-90, 0, -90);
                case CubeFace.MX:
                    return Quaternion.Euler(-90, 0, 90);
                case CubeFace.Y:
                    return Quaternion.Euler(0, 0, 0);
                case CubeFace.MY:
                    return Quaternion.Euler(0, 180, 180);
                case CubeFace.Z:
                    return Quaternion.AngleAxis(180, Vector3.forward) * Quaternion.AngleAxis(90, Vector3.right);
                case CubeFace.MZ:
                    return Quaternion.Euler(-90, 0, 0);
                default:
                    throw new UnhandledSwitchCaseException(faceNumber);
            }
        }

        public void RefreshView(CubeModel model)
        {
            currentModel = model;
            interior = new IBallController[model.X_SIZE, model.Y_SIZE, model.Z_SIZE];
            CreateBalls(model);
            CreateTiles(model);
        }

        private void CreateTiles(CubeModel model)
        {
            int widthFace;
            int heightFace;
            for (int faceNumber = 0; faceNumber < 6; faceNumber++)
            {
                GameObject face = new GameObject(faceNumber + "");
                face.transform.SetParent(transform);
                face.transform.localRotation = GetFaceRotation((CubeFace)faceNumber);
                face.transform.localPosition = GetFacePosition((CubeFace)faceNumber);
                switch (faceNumber)
                {
                    case (int)CubeFace.X:
                    case (int)CubeFace.MX:
                        widthFace = model.Z_SIZE;
                        heightFace = model.Y_SIZE;
                        break;
                    case (int)CubeFace.Y:
                    case (int)CubeFace.MY:
                        widthFace = model.X_SIZE;
                        heightFace = model.Z_SIZE;
                        break;
                    case (int)CubeFace.Z:
                    case (int)CubeFace.MZ:
                        widthFace = model.X_SIZE;
                        heightFace = model.Y_SIZE;
                        break;
                    default:
                        throw new UnhandledSwitchCaseException(faceNumber);
                }
                exterior[faceNumber] = new TileController[widthFace, heightFace];
                for (int x = 0; x < widthFace; x++)
                {
                    for (int y = 0; y < heightFace; y++)
                    {
                        TileData tileData = model.faces[faceNumber, x, y];
                        exterior[faceNumber][x, y] = TileCreator.CreateTile(tileData, GetTilePosition((CubeFace)faceNumber, x, y), 1);
                        exterior[faceNumber][x, y].transform.SetParent(face.transform, false);
                        model.filledObjectives.TryFillTile(exterior[faceNumber][x, y]);
                    }
                }
            }
        }

        private void CreateBalls(CubeModel data)
        {
            for (int x = 0; x < data.X_SIZE; x++)
            {
                for (int y = 0; y < data.Y_SIZE; y++)
                {
                    for (int z = 0; z < data.Z_SIZE; z++)
                    {
                        IBallController ball = BallCreator.GetBall(data.balls[x, y, z], 1,false);
                        ball.Init(x, y, z, this);



                        interior[x, y, z] = ball;
                    }
                }
            }
        }
    }
}
