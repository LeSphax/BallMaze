using BallMaze.Data;
using BallMaze.GameMechanics.Tiles;
using BallMaze.Inputs;
using UnityEngine;


namespace BallMaze.GameMechanics
{
    public abstract class BoardModel : MonoBehaviour
    {

        public const float SIZE_TILE_X = 1.1f;
        public const float SIZE_TILE_Y = 1.1f;

        protected BoardData boardData;

        protected BoardPosition[,] board;

        public int Width
        {
            get
            {
                return board.GetLength(0);
            }
        }

        public int Height
        {
            get
            {
                return board.GetLength(1);
            }
        }

        internal virtual void ReceiveInputCommand(BoardInputCommand inputCommand)
        {

        }

        public abstract void SetData(BoardData boardData);


        internal TileModel GetTile(int posX, int posY)
        {
            return board[posX, posY].tile;
        }

        internal IBallModel GetBrick(int posX, int posY)
        {
            return board[posX, posY].ball;
        }


        internal Vector3 GetWorldPosition(int posX, int posY)
        {
            return new Vector3(posX * SIZE_TILE_X, 0, posY * SIZE_TILE_Y);
        }

        internal void RemoveBrick(int x, int y)
        {
            board[x, y].ball = new EmptyBall();
        }

        internal void AddBrick(IBallModel brickModel, int x, int y)
        {
            board[x, y].ball = brickModel;
        }

        internal bool IsTileEmpty(int x, int y)
        {
            return board[x, y].ball.IsEmpty();
        }

        internal virtual void MoveBrick(int posX, int posY, int newPosX, int newPosY)
        {

        }
    }
}