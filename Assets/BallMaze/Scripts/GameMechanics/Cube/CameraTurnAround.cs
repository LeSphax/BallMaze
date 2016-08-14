using BallMaze;
using BallMaze.Inputs;
using UnityEngine;


public delegate void RotationChangeHandler(Vector3 newRotation);

public class CameraTurnAround : MonoBehaviour
{
    private bool moving;

    public GameObject Referent;
    public GameObject YObject;

    private float time;

    private Vector3 targetRotation;

    public event RotationChangeHandler RotationChanged;

    void Start()
    {
        InputManager inputManager = GameObject.FindGameObjectWithTag(Tags.InputManager).GetComponent<InputManager>();
        inputManager.DirectionEvent += TurnInDirection;
        moving = false;
        Init();
    }

    private void Init()
    {
        StartMove();
    }

    void Update()
    {
        if (moving)
        {
            time += 3 * Time.deltaTime;
            if (time > 1)
            {
                time = 1;
                moving = false;
            }
            YObject.transform.localEulerAngles = Vector3.Lerp(Vector3.zero, targetRotation, time);
            if (!moving)
            {
                Referent.transform.Rotate(YObject.transform.localEulerAngles);
                YObject.transform.localRotation = Quaternion.identity;
                targetRotation = Vector3.zero;
                SendRotation();
            }
        }
    }

    public void SendRotation()
    {
        if (RotationChanged != null)
            RotationChanged.Invoke(Referent.transform.localEulerAngles);
    }

    internal void TurnInDirection(Direction direction, bool moveBoard)
    {
        if (!moving && ! moveBoard)
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
        moving = true;
        time = 0;
    }
}
