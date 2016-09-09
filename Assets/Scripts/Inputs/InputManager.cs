using BallMaze.GameMechanics;
using UnityEngine;

namespace BallMaze.Inputs {
    public abstract class InputManager : MonoBehaviour
    {

        public event DirectionEventHandler MoveBoardEvent;
        public event DirectionEventHandler MoveCubeEvent;
        public event EmptyEventHandler ChangePerspectiveEvent;

        private Board board;

        protected virtual void Update()
        {
            if (CancelPressed())
            {
                Cancel();
            }
            else if (ResetPressed())
            {
                Reset();
            }
            else if (ChangePerspectivePressed())
            {
                if (ChangePerspectiveEvent != null)
                    ChangePerspectiveEvent.Invoke();
            }
            else
            {
                Direction direction = GetBoardDirection();
                if (direction != Direction.NONE && MoveBoardEvent != null)
                    MoveBoardEvent.Invoke(direction);
                direction = ChangeDirectionForCube(direction);
                if (direction != Direction.NONE && MoveCubeEvent != null)
                    MoveCubeEvent.Invoke(direction);
            }
        }

        public void Cancel()
        {
            board.ReceiveInputCommand(new CancelCommand());
        }

        public void Reset()
        {
            //board.ReceiveInputCommand(new ResetCommand());
        }

        public void LoadPreviousLevel()
        {
            // new PreviousLevelCommand(loader).Execute();
        }

        public void LoadNextLevel()
        {
            // new PreviousLevelCommand(loader).Execute();
        }

        public void Quit()
        {
            //new QuitCommand(saveManager, GameObject.FindGameObjectWithTag(Subject4087.Tags.Player).GetComponent<PlayerController>()).Execute();
        }

        public void SetBoard(Board board)
        {
            this.board = board;
        }


        protected abstract bool CancelPressed();

        protected abstract bool ResetPressed();

        protected abstract bool ChangePerspectivePressed();

        protected abstract Direction GetBoardDirection();

        protected virtual Direction ChangeDirectionForCube(Direction direction)
        {
            return direction;
        }


    }
}