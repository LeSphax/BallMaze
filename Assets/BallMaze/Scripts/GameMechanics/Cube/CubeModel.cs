using BallMaze.Data;
using BallMaze.Exceptions;
using BallMaze.GameMechanics.Balls;
using BallMaze.GameMechanics.Tiles;
using System;
using UnityEngine;

namespace BallMaze.GameMechanics
{ 
    public class CubeModel : MonoBehaviour
    {
        public const int NB_FACES = 6;

        protected IBallController[,,] interior;
        protected TileController[][,] exterior = new TileController[6][,];

        public int X_SIZE;
        public int Y_SIZE;
        public int Z_SIZE;

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
                return BASE_BOARD_SIZE / Mathf.Max(X_SIZE, Math.Max(Y_SIZE,Z_SIZE));
            }
        }


        void Start()
        {
            SetData(CubeData.GetDummyCubeData());
        }

        internal Vector3 GetBallPosition(int posX, int posY, int posZ)
        {
            return new Vector3(posX * TileSize, posY * TileSize, posZ * TileSize);
        }

        internal Vector3 GetTilePosition(int faceNumber, int sideIndex, int upIndex)
        {
            Vector3 result;
            switch (faceNumber)
            {
                case CubeFace.X:
                    result =  new Vector3(0.5f, 0.5f + upIndex, 0.5f + sideIndex);
                    break;
                case CubeFace.MX:
                    result = new Vector3(0.5f + X_SIZE, upIndex, 0.5f + Z_SIZE - sideIndex);
                    break;
                case CubeFace.Y:
                    result =  new Vector3(0.5f + sideIndex, 0.5f, 0.5f + upIndex);
                    break;
                case CubeFace.MY:
                    result =  new Vector3(0.5f + X_SIZE - sideIndex, 0.5f + Y_SIZE, 0.5f + upIndex);
                    break;
                case CubeFace.Z:
                    result =  new Vector3(0.5f + X_SIZE - sideIndex, 0.5f + upIndex, 0.5f);
                    break;
                case CubeFace.MZ:
                    result =  new Vector3(0.5f + sideIndex, 0.5f + upIndex, 0.5f + Z_SIZE);
                    break;
                default:
                    throw new UnhandledSwitchCaseException(faceNumber);
            }
            return result * TileSize;
        }

        internal Quaternion GetTileRotation(int faceNumber)
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

        public void SetData(CubeData data)
        {
            interior = new IBallController[data.X_SIZE, data.Y_SIZE, data.Z_SIZE];
            X_SIZE = data.X_SIZE;
            Y_SIZE = data.Y_SIZE;
            Z_SIZE = data.Z_SIZE;
            CreateBalls(data);
            CreateTiles(data);
        }

        private void CreateTiles(CubeData data)
        {
            int widthFace;
            int heightFace;
            for (int faceNumber = 0; faceNumber < 6; faceNumber++)
            {
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
                        exterior[faceNumber][x, y].transform.localRotation = GetTileRotation(faceNumber);
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
                        ball.Init(x, y, z, this);

                        interior[x, y, z] = ball;
                    }
                }
            }
        }
    }
}
