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
        public static float TURN_DURATION = 0.2f;

        private Stack<Turn> history;
        protected Turn currentTurn;

        private Queue<BoardInputCommand> inputs;

        public Dictionary<ObjectiveType, bool> objectivesFilled;

        public event ObjectiveListHandler NotifyObjectivesFilled;
        public event EmptyEventHandler LevelFinished;

        private int numberResetCommands = -1;

        internal override void ReceiveInputCommand(BoardInputCommand inputCommand)
        {
            if (state == State.IDLE)
            {
                ExecuteCommand(inputCommand);
            }
            else
            {
                inputs.Enqueue(inputCommand);
            }
        }

        private void ExecuteCommand(BoardInputCommand inputCommand)
        {
            inputCommand.SetModel(this);
            inputCommand.Execute();
            if (numberResetCommands == 0)
            {
                //Reset to original value
                TURN_DURATION = 0.2f;
            }
            if (numberResetCommands >= 0)
            {
                numberResetCommands--;
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
            // Avoiding a reset animation that lasts too long
            TURN_DURATION = Mathf.Min(0.2f, 1.0F / historySize);
            numberResetCommands = historySize;
            for (int i = 0; i <= historySize; i++)
            {
                ReceiveInputCommand(new CancelCommand());
            }
        }

        void Awake()
        {
            InitVariables();
        }

        protected virtual void Start()
        {
            InputManager inputManager = GameObjects.GetInputManager();
            inputManager.MoveBoardEvent += ReceiveDirection;
            inputManager.ReceivedCommand += ReceiveInputCommand;

            state = State.IDLE;
            FitBoardToCamera();
            //Fill the objectives if balls are already placed on them
            PlayTurn(Direction.NONE);
        }

        protected virtual void FinishTurn()
        {
            if (currentTurn.objectivesFilled.Count > 0)
            {
                if (NotifyObjectivesFilled != null)
                    NotifyObjectivesFilled.Invoke(currentTurn.objectivesFilled);
                // Not listened at the moment. Could replace the commands way
                //else
                //    Debug.LogError("This should be listened to");
            }
            if (state == State.PLAYING_TURN && currentTurn.WasUseful())
            {
                history.Push(currentTurn);
            }
            state = State.IDLE;
            currentTurn = null;
            if (!CheckLevelFinished() && inputs.Count > 0)
            {
                ExecuteCommand(inputs.Dequeue());
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
            boardData = data;
            board = new BoardPosition[boardData.X_SIZE, boardData.Y_SIZE];
            for (int x = 0; x < boardData.X_SIZE; x++)
            {
                for (int y = 0; y < boardData.Y_SIZE; y++)
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
            if (IsEmpty(newPosX, newPosY))
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

        protected virtual bool CheckLevelFinished()
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
                if (LevelFinished != null)
                    LevelFinished.Invoke();
                else
                    Debug.LogError("This should be listened to " + LevelFinished);
            }
            return won;
        }
    }
}