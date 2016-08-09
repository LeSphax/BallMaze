using System;
using BallMaze.Inputs;
using UnityEngine;

namespace BallMaze.GameMechanics
{
    public class CameraTurnAround : ACameraTurnAround
    {
        private new Camera camera;
        public GameObject center;
        public float distance;

        private float planeAngle;
        private float heightAngle;

        private float targetHeightAngle;
        private float targetPlaneAngle;

        private bool moving;

        private float time;

        void Start()
        {
            moving = false;
            InitAngles();
            SetPositionFromAngles();
        }

        private void InitAngles()
        {
            planeAngle = 0;
            heightAngle = Mathf.PI / 2;
            targetHeightAngle = heightAngle;
            targetPlaneAngle = planeAngle;
        }

        void Update()
        {
            if (moving)
            {
                time += Time.deltaTime;
                if (time > 1)
                {
                    time = 1;
                    moving = false;
                    //PerspectiveSwitcher.SetOrthographic(true);
                }
                planeAngle = Mathf.Lerp(planeAngle, targetPlaneAngle, time);
                heightAngle = Mathf.Lerp(heightAngle, targetHeightAngle, time);

                SetPositionFromAngles();
                //transform.LookAt(center.transform);
            }
        }

        private void SetPositionFromAngles()
        {
            float z = distance * Mathf.Cos(planeAngle) * Mathf.Sin(heightAngle);
            float x = distance * Mathf.Sin(planeAngle) * Mathf.Sin(heightAngle);

            float y = distance * Mathf.Cos(heightAngle);

            transform.localPosition = new Vector3(x, y, z);
            transform.localRotation = Quaternion.Euler((heightAngle -Mathf.PI/2) * Mathf.Rad2Deg, planeAngle * Mathf.Rad2Deg, 0);
        }

        internal override void TurnInDirection(Direction direction)
        {
            if (!moving)
            {
                switch (direction)
                {
                    case Direction.UP:
                        targetHeightAngle = heightAngle - 90 * Mathf.Deg2Rad;
                        break;
                    case Direction.DOWN:
                        targetHeightAngle = heightAngle + 90 * Mathf.Deg2Rad;
                        break;
                    case Direction.RIGHT:
                        targetPlaneAngle = planeAngle - 90 * Mathf.Deg2Rad;
                        break;
                    case Direction.LEFT:
                        targetPlaneAngle = planeAngle + 90 * Mathf.Deg2Rad;
                        break;
                    case Direction.NONE:
                        break;
                    default:
                        break;
                }
                if (direction != Direction.NONE)
                {
                    Debug.Log("PLANE" + planeAngle);
                    Debug.Log("HEIGHT" + heightAngle);
                    StartMove();
                }
            }
        }

        public int GetCurrentFace()
        {
            if (Mathf.Approximately(0, targetHeightAngle.mod(Mathf.PI * 2)))
            {
                return CubeFace.Y;
            }
            else if (Mathf.Approximately(Mathf.PI, targetHeightAngle.mod(Mathf.PI * 2)))
            {
                return CubeFace.MY;
            }
            else
            {
                if (Mathf.Approximately(0, targetPlaneAngle.mod(Mathf.PI * 2)))
                {
                    return CubeFace.Z;
                }
                else if (Mathf.Approximately(Mathf.PI/2, targetPlaneAngle.mod(Mathf.PI*2)))
                {
                    return CubeFace.X;
                }
                else if (Mathf.Approximately(Mathf.PI, targetPlaneAngle.mod(Mathf.PI * 2)))
                {
                    return CubeFace.MZ;
                }
                else if (Mathf.Approximately(3 * Mathf.PI/2, targetPlaneAngle.mod(Mathf.PI * 2)))
                {
                    return CubeFace.MX;
                }
                else
                {
                    throw new Exception("The angles don't match any face : plane " + targetPlaneAngle + "height " + targetHeightAngle);
                }
            }
        }

        private void StartMove()
        {
            moving = true;
            time = 0;
            //  PerspectiveSwitcher.SetOrthographic(false);
        }
    }
}
