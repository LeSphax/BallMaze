using BallMaze;
using BallMaze.Inputs;
using UnityEngine;


public delegate void RotationChangeHandler(Vector3 newRotation);

public class CameraTurnAround : MonoBehaviour
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

    public event EmptyEventHandler RotationChangeStart;
    public event RotationChangeHandler RotationChanged;

    void Start()
    {
        GameObjects.GetLevelLoader().LevelChanged += Init;
        InputManager inputManager = GameObject.FindGameObjectWithTag(Tags.InputManager).GetComponent<InputManager>();
        inputManager.DirectionEvent += TurnInDirection;
        moving = false;
    }

    public void Init()
    {
        Referent.transform.localRotation = Quaternion.identity;
        YObject.transform.localRotation = Quaternion.identity;
        targetRotation = Vector3.zero;
        EndMove();
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

    private void EndMove()
    {
        ApplyRotationsAndReset();
        SendRotationChangedEvent();
        SetOrtho();
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

    public void SendRotationChangeStartedEvent()
    {
        if (RotationChangeStart != null)
            RotationChangeStart.Invoke();
    }

    internal void TurnInDirection(Direction direction, bool moveBoard)
    {
        if (!moving && !moveBoard)
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
                StartMove();
            }
        }
    }

    public static CubeFace GetFace(Vector3 rotation)
    {
        Vector3 forward = Quaternion.Euler(rotation) * Vector3.forward;
        float treshold = 0.1f;
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
            Debug.LogError("The camera should be aligned with a face");
            return CubeFace.NONE;
        }
    }

    private void StartMove()
    {
        SendRotationChangeStartedEvent();
        Invoke("SetPerspective", GetComponentsFadeAnimation.timeCube);
    }

    public void OrthoOn()
    {
    }

    public void PerspectiveOn()
    {
        time = 0;
        moving = true;
    }

    private void SetOrtho()
    {
        perspectiveSwitcher.SetOrthographic(true, OrthoOn);
    }

    private void SetPerspective()
    {
        perspectiveSwitcher.SetOrthographic(false, PerspectiveOn);

    }
}
