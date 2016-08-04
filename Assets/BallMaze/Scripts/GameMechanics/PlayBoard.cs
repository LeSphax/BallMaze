using BallMaze.Data;
using BallMaze.GameManagement;
using BallMaze.GameMechanics.Balls;
using BallMaze.GameMechanics.Tiles;
using BallMaze.GameMechanics.Turns;
using BallMaze.Inputs;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BallMaze.GameMechanics
{
    public class PlayBoard : Board
    {
        private enum State
        {
            IDLE,
            PLAYING_TURN,
            WON,
            CANCELLING_TURN
        }
        private State state;
        public const float TURN_DURATION = 0.2f;

        private Stack<Turn> history;
        private Turn currentTurn;

        private Queue<BoardInputCommand> inputs;


        internal override void ReceiveInputCommand(BoardInputCommand inputCommand)
        {
            if (state == State.IDLE)
            {
                inputCommand.SetModel(this);
                inputCommand.Execute();
            }
            else
            {
                inputs.Enqueue(inputCommand);
            }
        }

        internal void CancelLastMovement()
        {
            if (history.Count > 0)
            {
                state = State.CANCELLING_TURN;
                currentTurn = history.Pop();
                currentTurn.UnPlay();
            }
        }

        internal void Reset()
        {
            int historySize = history.Count;
            for (int i = 0; i <= historySize; i++)
            {
                ReceiveInputCommand(new CancelCommand());
            }
        }

        void Awake()
        {
            InitVariables();
        }

        void Start()
        {
            state = State.IDLE;
            FitBoardToCamera();
        }

        private void FinishTurn()
        {
            if (state == State.PLAYING_TURN && currentTurn.WasUseful())
            {
                history.Push(currentTurn);
            }
            state = State.IDLE;
            currentTurn = null;
            if (!CheckIfWon() && inputs.Count > 0)
            {
                BoardInputCommand command = inputs.Dequeue();
                command.SetModel(this);
                command.Execute();
            }
        }


        private void FitBoardToCamera()
        {
            float worldWidth = TileXSize * Width;
            float worldHeight = TileYSize * Height;

            float posX = -worldWidth / 2;
            float posY = 0;// -Mathf.Max(worldWidth / 2, worldHeight / 2);
            float posZ = -worldHeight / 2;
            posZ += 1.2f;
            transform.localPosition = new Vector3(posX, posY, posZ);
        }

        public override void SetData(BoardData data)
        {
            BoardData boardData = data;
            board = new BoardPosition[boardData.Width, boardData.Height];
            for (int x = 0; x < boardData.Width; x++)
            {
                for (int y = 0; y < boardData.Height; y++)
                {
                    Vector3 position = GetWorldPosition(x, y);

                    TileController tile = TileCreator.CreateTile(boardData.tiles[x, y], position, SizeRatio);
                    tile.transform.SetParent(transform, false);

                    IBallController ball = BallCreator.GetBall(boardData.balls[x, y], SizeRatio);
                    ball.Init(x, y, this);

                    board[x, y] = new BoardPosition(tile, ball);
                }
            }
        }

        private void InitVariables()
        {
            history = new Stack<Turn>();
            inputs = new Queue<BoardInputCommand>();
        }

        internal override void MoveBrick(int posX, int posY, int newPosX, int newPosY)
        {
            if (IsTileEmpty(newPosX, newPosY))
            {
                board[newPosX, newPosY].ball = board[posX, posY].ball;
                RemoveBrick(posX, posY);
            }
            else
            {
                Debug.LogError("PlayBoardModel.MoveBrick" + board[posX, posY].ball.ToString() + "(" + posX + "," + posY + "," + newPosX + "," + newPosY + ") : The target tile should be empty");
                Debug.DebugBreak();
            }
        }

        public void PlayTurn(Direction direction)
        {
            state = State.PLAYING_TURN;
            currentTurn = new Turn(this);
            currentTurn.FinishedPlaying += new EmptyEventHandler(FinishTurn);
            currentTurn.Play(direction);

        }

        private void UndoTurn(Turn turn)
        {
            turn.UnPlay();
        }

        private bool CheckIfWon()
        {
            bool won = true;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (!board[x, y].tile.IsFilled())
                    {
                        won = false;
                    }
                }
            }
            if (won)
            {
                state = State.WON;
                GetComponent<LevelManager>().LevelFinished();
            }
            return won;
        }
    }
}