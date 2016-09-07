using BallMaze.GameMechanics;
using UnityEngine;

namespace BallMaze.Inputs
{

    public delegate void DirectionEventHandler(Direction direction);

    public class PCInputManager : InputManager
    {
        protected override bool ChangePerspectivePressed()
        {
            return Input.GetButtonDown(InputButtonNames.CHANGE_PERSPECTIVE);
        }

        protected override bool ResetPressed()
        {
            return Input.GetButtonDown(InputButtonNames.RESET);
        }

        protected override bool CancelPressed()
        {
            return Input.GetButtonDown(InputButtonNames.CANCEL) || Input.GetKeyDown(KeyCode.Escape);
        }


        protected override Direction GetDirection()
        {
            if (Input.GetButtonDown(InputButtonNames.RotateUp))
            {
                return Direction.UP;
            }
            else if (Input.GetButtonDown(InputButtonNames.RotateDown))
            {
                return Direction.DOWN;
            }
            else if (Input.GetButtonDown(InputButtonNames.RotateRight))
            {
                return Direction.RIGHT;
            }
            else if (Input.GetButtonDown(InputButtonNames.RotateLeft))
            {
                return Direction.LEFT;
            }
            return Direction.NONE;
        }
    }


}