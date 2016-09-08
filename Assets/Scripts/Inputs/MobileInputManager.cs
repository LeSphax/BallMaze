using System;
using System.Collections.Generic;
using UnityEngine;

namespace BallMaze.Inputs
{
    public class MobileInputManager : InputManager
    {
        private enum SwipeState
        {
            IDLE,
            PRESSED,
        }

        private SwipeState swipeState;

        private int previousTouchCount;

        public float minSwipeLength = 200f;
        Vector2 firstPressPos;
        Vector2 secondPressPos;
        Vector2 currentSwipe;

        void Start()
        {
            swipeState = SwipeState.IDLE;
        }

        protected override Direction GetBoardDirection()
        {
            return DetectSwipe();
        }

        protected override Direction ChangeDirectionForCube(Direction direction)
        {
            return direction.Opposite();
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

        protected override bool CancelPressed()
        {
            return false;
        }

        protected override bool ResetPressed()
        {
            return false;
        }

        protected override bool ChangePerspectivePressed()
        {
            if (Input.touchCount == 2 && previousTouchCount<2)
            {
                previousTouchCount = Input.touchCount;
                return true;
            }
            previousTouchCount = Input.touchCount;
            return false;
        }
    }



    //public Direction DetectMouseSwipe()
    //{
    //    switch (swipeState)
    //    {
    //        case SwipeState.IDLE:
    //            if (Input.GetMouseButtonDown(0))
    //            {
    //                firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    //                swipeState = SwipeState.PRESSED;
    //            }
    //            break;
    //        case SwipeState.PRESSED:
    //            if (Input.GetMouseButtonUp(0))
    //            {
    //                swipeState = SwipeState.IDLE;
    //                secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    //                currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

    //                // Make sure it was a legit swipe, not a tap
    //                if (currentSwipe.magnitude < minSwipeLength)
    //                {
    //                    return Direction.NONE;
    //                }

    //                currentSwipe.Normalize();
    //                if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
    //                {
    //                    return Direction.UP;
    //                }
    //                else if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
    //                {
    //                    return Direction.DOWN;
    //                }
    //                else if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
    //                {
    //                    return Direction.LEFT;
    //                }
    //                else if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
    //                {
    //                    return Direction.RIGHT;
    //                }
    //                else
    //                {
    //                    return Direction.NONE;
    //                }
    //            }
    //            break;
    //    }
    //    return Direction.NONE;
    //}
}