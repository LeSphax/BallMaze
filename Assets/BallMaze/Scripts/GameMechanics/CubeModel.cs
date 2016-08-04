using BallMaze.Data;
using BallMaze.GameMechanics.Balls;
using BallMaze.GameMechanics.Tiles;
using UnityEngine;

namespace BallMaze.GameMechanics
{
    public class CubeModel : MonoBehaviour
    {
        protected Position[,,] board;

        void Start()
        {
            SetData(CubeData.GetDummyCubeData());
        }

        internal Vector3 GetWorldPosition(int posX, int posY, int posZ)
        {
            return new Vector3(posX * SizeElements.SIZE_TILE_X, posY * SizeElements.SIZE_TILE_Y, posZ * SizeElements.SIZE_TILE_Z);
        }

        public void SetData(CubeData data)
        {
            board = new Position[data.Width, data.Height, data.Depth];
            for (int x = 0; x < data.Width; x++)
            {
                for (int y = 0; y < data.Height; y++)
                {
                    for (int z = 0; z < data.Depth; z++)
                    {
                        Vector3 position = GetWorldPosition(x, y, z);

                        TileController tile = TileCreator.CreateTile(data.tiles[x, y, z], position,1);
                        tile.transform.SetParent(transform, false);

                        IBallController ball = BallCreator.GetBall(data.balls[x, y, z],1);
                        ball.Init(x, y, z, this);

                        board[x, y, z] = new Position(tile, ball);
                    }
                }
            }
        }
    }
}
