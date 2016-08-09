using BallMaze.GameMechanics.Tiles;
using BallMaze.Inputs;
using System;
using UnityEngine;

namespace BallMaze.GameMechanics.ObjectiveBall
{
    [Serializable]
    public class ObjectiveBallController : BallController<ObjectiveBallView>
    {
        private enum State
        {
            IDLE,
            ANIMATING,
        }

        private State state;

        private Vector3 targetPosition;

        protected override void Awake()
        {
            base.Awake();
            state = State.IDLE;
        }

        protected override void InitView(bool cube = false)
        {
            view.FinishedAnimating += new EmptyEventHandler(ViewReachedTarget);
            view.SetPosition(GetWorldPosition(), cube);
        }

        public void SetPosition(int newPosX, int newPosY)
        {
            boardModel.MoveBrick(posX, posY, newPosX, newPosY);
            posX = newPosX;
            posY = newPosY;
            view.SetTarget(GetWorldPosition());
        }


        public override void Move(Direction direction)
        {
            if (state == State.ANIMATING)
            {
                SendMoveError();
            }
            else
            {
                Vector2 newBoardTarget = new Vector2(posX, posY);
                if (direction == Direction.UP)
                {
                    for (int i = posY + 1; i < boardModel.Height; i++)
                    {
                        if (boardModel.IsEmpty(posX, i))
                        {
                            newBoardTarget = new Vector2(posX, i);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else if (direction == Direction.DOWN)
                {
                    for (int i = posY - 1; i >= 0; i--)
                    {
                        if (boardModel.IsEmpty(posX, i))
                        {
                            newBoardTarget = new Vector2(posX, i);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else if (direction == Direction.RIGHT)
                {
                    for (int i = posX + 1; i < boardModel.Width; i++)
                    {
                        if (boardModel.IsEmpty(i, posY))
                        {
                            newBoardTarget = new Vector2(i, posY);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else if (direction == Direction.LEFT)
                {
                    for (int i = posX - 1; i >= 0; i--)
                    {
                        if (boardModel.IsEmpty(i, posY))
                        {
                            newBoardTarget = new Vector2(i, posY);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                if (newBoardTarget.x != posX || newBoardTarget.y != posY)
                {
                    state = State.ANIMATING;
                    SetPosition((int)newBoardTarget.x, (int)newBoardTarget.y);
                }
                else
                {
                    state = State.IDLE;
                    RaiseFinishedAnimating();
                }
            }

        }

        private void SendMoveError()
        {
            Debug.LogError("Brick " + gameObject.name + "The board should wait for the movement to finish before sending new move commands");
        }

        public override void MoveBack(int oldPosX, int oldPosY)
        {
            if (state == State.ANIMATING)
            {
                SendMoveError();
            }
            else
            {
                state = State.ANIMATING;
                SetPosition(oldPosX, oldPosY);
            }
        }

        internal void ViewReachedTarget()
        {
            state = State.IDLE;
            RaiseFinishedAnimating();
        }

        public override void FillObjective()
        {
            boardModel.RemoveBrick(posX, posY);
            view.CompleteView(boardModel.GetTile(posX, posY));
        }

        public override void UnFillObjective()
        {
            boardModel.AddBrick(this, posX, posY);
            view.UnCompleteView(boardModel.GetTile(posX, posY));
        }

        public override string ToString()
        {
            return "Brick " + objectiveType;
        }

        public override bool IsMoving()
        {
            return true;
        }

        private GameObject GetCorrespondingTile()
        {
            foreach (GameObject tile in GameObject.FindGameObjectsWithTag(Tags.SyncedTile))
            {
                if (tile.GetComponent<TileController>().GetObjectiveType() == GetObjectiveType())
                {
                    return tile;
                }
            }
            Debug.LogError("This ball has no corresponding tile !!");
            return null;
        }
    }
}

