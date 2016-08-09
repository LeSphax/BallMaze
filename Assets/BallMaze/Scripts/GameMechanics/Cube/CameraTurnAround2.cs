using System;
using BallMaze.Inputs;
using UnityEngine;

namespace BallMaze.GameMechanics
{
    public class CameraTurnAround2 : ACameraTurnAround
    {
        private new Camera camera;
        public GameObject center;
        public float distance;

        private Quaternion startRotation;
        private Quaternion targetRotation;

        private Vector3 startAngles;

        private float planeAngle;
        private float heightAngle;
        private float headAngle;

        private float targetPlaneAngle;
        private float targetHeightAngle;
        private float targetHeadAngle;

        private CameraPosition position;

        private bool moving;

        private float time;

        void Start()
        {
            moving = false;
            Init();
            SetPositionFromAngles();
        }

        private void Init()
        {
            position = new CameraPosition();
            position.InitNormal();
            MoveToAngles();
            planeAngle = targetPlaneAngle;
            heightAngle = targetHeightAngle;
            headAngle = targetHeadAngle;
            StartMove();
        }

        private void MoveToAngles()
        {
            Vector3 angles = position.ToAngles();
            Debug.Log(angles * Mathf.Rad2Deg);
            CropAngle(ref planeAngle, ref targetPlaneAngle, angles.x);
            CropAngle(ref heightAngle, ref targetHeightAngle, angles.y);
            CropAngle(ref headAngle, ref targetHeadAngle, angles.z);

            startAngles = new Vector3(planeAngle,heightAngle,headAngle);
            startRotation = transform.localRotation;
            targetRotation = GetTargetRotation(position.face);
            Debug.Log(targetRotation.eulerAngles);
        }

        private void CropAngle(ref float currentAngle, ref float targetAngle, float newAngle)
        {
            if (currentAngle - newAngle < -Mathf.PI)
            {
                targetAngle = newAngle - Mathf.PI * 2;
            }
            else
            {
                targetAngle = newAngle;
                if (currentAngle - newAngle > Mathf.PI)
                {
                    currentAngle -= Mathf.PI * 2;
                }
            }
        }

        void Update()
        {
            if (moving)
            {
                time += 3* Time.deltaTime;
                if (time > 1)
                {
                    time = 1;
                    moving = false;
                    //PerspectiveSwitcher.SetOrthographic(true);
                }
                planeAngle = Mathf.Lerp(startAngles.x, targetPlaneAngle, time);
                heightAngle = Mathf.Lerp(startAngles.y, targetHeightAngle, time);
                headAngle = Mathf.Lerp(startAngles.z, targetHeadAngle, time);
                transform.localRotation = Quaternion.Lerp(startRotation, targetRotation, time);

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
        }

        private Quaternion GetTargetRotation(int face)
        {
            switch (face)
            {
                case CubeFace.X:
                case CubeFace.MX:
                    return Quaternion.Euler(headAngle * Mathf.Rad2Deg, planeAngle * Mathf.Rad2Deg, (heightAngle - Mathf.PI / 2) * Mathf.Rad2Deg);
                case CubeFace.Z:
                case CubeFace.MZ:
                    return Quaternion.Euler((heightAngle - Mathf.PI / 2) * Mathf.Rad2Deg, planeAngle * Mathf.Rad2Deg, headAngle * Mathf.Rad2Deg);
                case CubeFace.Y:
                case CubeFace.MY:
                    return Quaternion.Euler((heightAngle - Mathf.PI/2) * Mathf.Rad2Deg, planeAngle * Mathf.Rad2Deg, 0);
                default:
                    throw new UnhandledSwitchCaseException(face);
            }
        }

        internal override void TurnInDirection(Direction direction)
        {
            if (!moving)
            {
                switch (direction)
                {
                    case Direction.UP:
                        position.GoUp();
                        break;
                    case Direction.DOWN:
                        position.GoDown();
                        break;
                    case Direction.RIGHT:
                        position.GoRight();
                        break;
                    case Direction.LEFT:
                        position.GoLeft();
                        break;
                    case Direction.NONE:
                        break;
                    default:
                        throw new UnhandledSwitchCaseException(direction);
                }
                if (direction != Direction.NONE)
                {
                    MoveToAngles();
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
                else if (Mathf.Approximately(Mathf.PI / 2, targetPlaneAngle.mod(Mathf.PI * 2)))
                {
                    return CubeFace.X;
                }
                else if (Mathf.Approximately(Mathf.PI, targetPlaneAngle.mod(Mathf.PI * 2)))
                {
                    return CubeFace.MZ;
                }
                else if (Mathf.Approximately(3 * Mathf.PI / 2, targetPlaneAngle.mod(Mathf.PI * 2)))
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
