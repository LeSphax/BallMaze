using BallMaze.Data;
using BallMaze.GameMechanics.Tiles;
using BallMaze.Inputs;
using UnityEngine;


namespace BallMaze.GameMechanics
{
    public abstract class Board : MonoBehaviour
    {

        protected virtual void Start()
        {
            InputManager inputManager = GameObjects.GetInputManager();
            inputManager.DirectionEvent += ReceiveDirection;
        }

        public const float BASE_TILE_SIZE_X = 1.1f;
        public const float BASE_TILE_SIZE_Y = 1.1f;
        public float TileXSize
        {
            get
            {
                return BASE_TILE_SIZE_X * SizeRatio;
            }

        }
        public float TileYSize
        {
            get
            {
                return BASE_TILE_SIZE_Y * SizeRatio;
            }
        }
        private const float BASE_BOARD_SIZE = 3;

        public virtual float SizeRatio
        {
            get
            {
                return BASE_BOARD_SIZE / Mathf.Max(Width, Height);
            }
        }

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

        internal void ReceiveDirection(Direction direction, bool moveBoard)
        {
            if (moveBoard)
                ReceiveInputCommand(new MoveCommand(direction));
        }

        internal virtual void ReceiveInputCommand(BoardInputCommand inputCommand)
        {

        }

        public abstract void SetData(BoardData boardData);


        internal TileController GetTile(int posX, int posY)
        {
            return board[posX, posY].tile;
        }

        internal IBallController GetBrick(int posX, int posY)
        {
            return board[posX, posY].ball;
        }


        internal Vector3 GetWorldPosition(int posX, int posY)
        {
            return new Vector3(posX * TileXSize, 0, posY * TileYSize);
        }

        internal void RemoveBrick(int x, int y)
        {
            board[x, y].ball = new EmptyBallController();
        }

        internal void AddBrick(IBallController brickModel, int x, int y)
        {
            board[x, y].ball = brickModel;
        }

        internal bool IsEmpty(int x, int y)
        {
            return board[x, y].ball.IsEmpty();
        }

        internal virtual void MoveBrick(int posX, int posY, int newPosX, int newPosY)
        {

        }

        void OnDestroy()
        {

            InputManager inputManager = GameObjects.GetInputManager();
            if (inputManager != null)
                inputManager.DirectionEvent -= ReceiveDirection;
        }
    }
}