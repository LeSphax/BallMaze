using BallMaze.Data;
using BallMaze.GameMechanics;
using BallMaze.GameMechanics.Balls;
using BallMaze.GameMechanics.Tiles;
using UnityEngine;

namespace BallMaze.LevelCreation
{
    public class EditorBoard : Board
    {

        protected override void Start()
        {
            base.Start();
            board = new BoardPosition[0, 0];
        }

        public override float SizeRatio
        {
            get
            {
               return 1;
            }
        }

        public override void SetData(BoardData data)
        {
            boardData = data;
            DeleteBoard();
            board = new BoardPosition[data.Width, data.Height];
            for (int x = 0; x < data.Width; x++)
            {
                for (int y = 0; y < data.Height; y++)
                {
                    if (data.tiles[x, y] != null)
                    {
                        Vector3 position = GetWorldPosition(x, y);

                        TileController tile = TileCreator.CreateTile(data.tiles[x, y], position, 1);
                        tile.transform.SetParent(transform, false);

                        IBallController ball = BallCreator.GetBall(data.balls[x, y], 1);
                        ball.Init(x, y, this);

                        board[x, y] = new BoardPosition(tile, ball);
                    }
                }
            }
        }

        private void DeleteBoard()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (board[x, y] != null)
                    {
                        Destroy(board[x, y].tile.gameObject);
                        board[x, y].ball.Destroy();
                    }
                }
            }
        }
    }
}