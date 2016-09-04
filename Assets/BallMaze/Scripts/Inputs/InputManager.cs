using BallMaze.GameMechanics;
using UnityEngine;

namespace BallMaze.Inputs
{


    public class InputManager : MonoBehaviour
    {
        public delegate void DirectionEventHandler(Direction direction, bool moveBoard);
        private Board board;

        private enum SwipeState
        {
            IDLE,
            PRESSED,
        }

        private SwipeState swipeState;

        public float minSwipeLength = 200f;
        Vector2 firstPressPos;
        Vector2 secondPressPos;
        Vector2 currentSwipe;

        public event DirectionEventHandler DirectionEvent;

        private bool moveBoard = false;

        void Start()
        {
            //loader = GameObject.FindGameObjectWithTag(Tags.BallMazeController).GetComponent<LevelLoader>();
            //if (GameObject.FindGameObjectWithTag(Tags.GameData))
            //    saveManager = GameObject.FindGameObjectWithTag(Tags.GameData).GetComponent<SaveManager>();
            swipeState = SwipeState.IDLE;
        }

        protected virtual void Update()
        {
            if (Input.GetButtonDown(InputButtonNames.CANCEL) || Input.GetKeyDown(KeyCode.Escape))
            {
                Cancel();
            }
            else if (Input.GetButtonDown(InputButtonNames.RESET))
            {
                Reset();
            }
            else
            {
                Direction direction = GetDirection();
                if (direction != Direction.NONE)
                    DirectionEvent.Invoke(direction, moveBoard);
            }
        }

        public void Cancel()
        {
            board.ReceiveInputCommand(new CancelCommand());
        }

        public void Reset()
        {
            board.ReceiveInputCommand(new ResetCommand());
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

        public Direction DetectMouseSwipe()
        {
            switch (swipeState)
            {
                case SwipeState.IDLE:
                    if (Input.GetMouseButtonDown(0))
                    {
                        firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                        swipeState = SwipeState.PRESSED;
                    }
                    break;
                case SwipeState.PRESSED:
                    if (Input.GetMouseButtonUp(0))
                    {
                        swipeState = SwipeState.IDLE;
                        secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                        currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                        // Make sure it was a legit swipe, not a tap
                        if (currentSwipe.magnitude < minSwipeLength)
                        {
                            return Direction.NONE;
                        }

                        currentSwipe.Normalize();
                        if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                        {
                            return Direction.UP;
                        }
                        else if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                        {
                            return Direction.DOWN;
                        }
                        else if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                        {
                            return Direction.LEFT;
                        }
                        else if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                        {
                            return Direction.RIGHT;
                        }
                        else
                        {
                            return Direction.NONE;
                        }
                    }
                    break;
            }
            return Direction.NONE;
        }

        public Direction DetectSwipe()
        {
            if (Input.touches.Length > 0)
            {
                Touch t = Input.GetTouch(0);

                switch (swipeState)
                {
                    case SwipeState.IDLE:

                        if (t.phase == TouchPhase.Began)
                        {
                            firstPressPos = new Vector2(t.position.x, t.position.y);
                            swipeState = SwipeState.PRESSED;
                        }
                        break;
                    case SwipeState.PRESSED:
                        if (t.phase == TouchPhase.Ended)
                        {
                            swipeState = SwipeState.IDLE;
                            secondPressPos = new Vector2(t.position.x, t.position.y);
                            currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

                            // Make sure it was a legit swipe, not a tap
                            if (currentSwipe.magnitude < minSwipeLength)
                            {
                                return Direction.NONE;
                            }

                            currentSwipe.Normalize();

                            // Swipe up
                            if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                            {
                                return Direction.UP;
                            }
                            else if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                            {
                                return Direction.DOWN;
                            }
                            else if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                            {
                                return Direction.LEFT;
                            }
                            else if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                            {
                                return Direction.RIGHT;
                            }
                            else
                            {
                                return Direction.NONE;
                            }
                        }
                        break;
                }
            }
            return Direction.NONE;
        }


        public Direction GetDirection()
        {
            if (Input.touchSupported && Application.platform != RuntimePlatform.WebGLPlayer)
                return DetectSwipe();
            else if (GetDirectionWithButtons() != Direction.NONE)
            {
                return GetDirectionWithButtons();
            }
            else
                return DetectMouseSwipe();
        }

        private Direction GetDirectionWithButtons()
        {
            moveBoard = true;
            if (Input.GetButtonDown(InputButtonNames.UP))
            {
                return Direction.UP;
            }
            else if (Input.GetButtonDown(InputButtonNames.DOWN))
            {
                return Direction.DOWN;
            }
            else if (Input.GetButtonDown(InputButtonNames.RIGHT))
            {
                return Direction.RIGHT;
            }
            else if (Input.GetButtonDown(InputButtonNames.LEFT))
            {
                return Direction.LEFT;
            }
            moveBoard = false;
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

        public void SetBoard(Board board)
        {
            this.board = board;
        }
    }


}