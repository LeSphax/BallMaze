using BallMaze;
using BallMaze.Inputs;
using UnityEngine;

namespace BallMaze.Cube
{
    public delegate void RotationChangeHandler(Vector3 newRotation);


    public class CameraController : MonoBehaviour
    {
        private const float turningSpeed = 3f;
        private bool moving;

        // The object that keeps the current rotation matrix
        public GameObject Referent;
        //The object that rotates during the movement
        public GameObject YObject;
        public PerspectiveSwitcher perspectiveSwitcher;
        public GameObject Camera;

        public GameObject cubeView;
        public GameObject slice;

        private float time;

        private Vector3 targetRotation = Vector3.zero;

        public event EmptyEventHandler PerspectiveActivated;
        public event RotationChangeHandler RotationChanged;

        private CameraStateMachine stateMachine;

        void Awake()
        {
            stateMachine = gameObject.AddComponent<CameraStateMachine>();
        }

        void Start()
        {
            moving = false;
        }

        public void Init()
        {
            stateMachine.handleEvent(new E_InitCamera());
        }

        public void Reset()
        {
            Referent.transform.localRotation = Quaternion.identity;
            YObject.transform.localRotation = Quaternion.identity;
            targetRotation = Vector3.zero;
            FadeIn();
        }

        void Update()
        {
            if (moving)
            {
                time += turningSpeed * Time.deltaTime;
                if (time > 1)
                {
                    time = 1;
                    moving = false;
                }
                YObject.transform.localEulerAngles = Vector3.Lerp(Vector3.zero, targetRotation, time);
                if (!moving)
                {
                    EndMove();
                }
            }
        }

        public void FadeIn()
        {
            SendRotationChangedEvent();
            OrthographicCamera();
        }

        public void FadeOut()
        {
            SendPerspectiveChangedEvent();
            Invoke("PerspectiveCamera", GetComponentsFadeAnimation.timeCube);
        }

        private void EndMove()
        {
            ApplyRotationsAndReset();
            stateMachine.handleEvent(new E_FinishedAnimating());
        }

        //Callback when orthographic is activated
        public void OrthoOn()
        {
            stateMachine.handleEvent(new E_FinishedAnimating());
        }

        //Callback when perspective is activated
        public void PerspectiveOn()
        {
            stateMachine.handleEvent(new E_FinishedAnimating());
        }

        private void StartMoving()
        {
            time = 0;
            moving = true;
        }

        private void OrthographicCamera()
        {
            perspectiveSwitcher.SetOrthographic(true, OrthoOn);
        }

        private void PerspectiveCamera()
        {
            perspectiveSwitcher.SetOrthographic(false, PerspectiveOn);

        }

        //Add the last rotation to the current rotation matrix then resets the objects that are used during the movement
        private void ApplyRotationsAndReset()
        {
            Referent.transform.Rotate(YObject.transform.localEulerAngles);
            YObject.transform.localRotation = Quaternion.identity;
            targetRotation = Vector3.zero;
        }

        public void SendRotationChangedEvent()
        {
            if (RotationChanged != null)
                RotationChanged.Invoke((Referent.transform.localRotation * Quaternion.Euler(targetRotation)).eulerAngles);
        }

        public void SendPerspectiveChangedEvent()
        {
            if (PerspectiveActivated != null)
                PerspectiveActivated.Invoke();
        }

        internal void TurnInDirection(Direction direction)
        {
            if (!moving)
            {
                switch (direction)
                {
                    case Direction.UP:
                        targetRotation.x = 90;
                        break;
                    case Direction.DOWN:
                        targetRotation.x = -90;
                        break;
                    case Direction.RIGHT:
                        targetRotation.y = -90;
                        break;
                    case Direction.LEFT:
                        targetRotation.y = 90;
                        break;
                    case Direction.NONE:
                        break;
                    default:
                        throw new UnhandledSwitchCaseException(direction);
                }
                if (direction != Direction.NONE)
                {
                    StartMoving();
                }
            }
        }

        public static CubeFace GetFace(Vector3 rotation)
        {
            Vector3 forward = Quaternion.Euler(rotation) * Vector3.forward;
            float treshold = 1.0f;
            if (Vector3.Angle(forward, Vector3.right) < treshold)
            {
                return CubeFace.MX;
            }
            else if (Vector3.Angle(forward, Vector3.left) < treshold)
            {
                return CubeFace.X;
            }
            else if (Vector3.Angle(forward, Vector3.forward) < treshold)
            {
                return CubeFace.MZ;
            }
            else if (Vector3.Angle(forward, Vector3.back) < treshold)
            {
                return CubeFace.Z;
            }
            else if (Vector3.Angle(forward, Vector3.up) < treshold)
            {
                return CubeFace.MY;
            }
            else if (Vector3.Angle(forward, Vector3.down) < treshold)
            {
                return CubeFace.Y;
            }
            else
            {
                Debug.LogError("The camera should be aligned with a face, the angle is " + Vector3.Angle(forward, Vector3.forward));
                return CubeFace.NONE;
            }
        }

    }
}