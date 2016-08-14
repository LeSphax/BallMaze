using BallMaze;
using BallMaze.GameManagement;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    private Level3DManager loader;
    public CubeModel model;

    void Start()
    {
        model.SetDummyCubeModel();
        CameraTurnAround cameraController = GameObject.FindGameObjectWithTag(Tags.CameraController).GetComponent<CameraTurnAround>();
        cameraController.RotationChanged += RotationChanged;
        loader = GameObject.FindGameObjectWithTag(Tags.BallMazeController).GetComponent<Level3DManager>();

        cameraController.SendRotation();
    }

    public void RotationChanged(Vector3 rotation)
    {
        loader.CreateSlice(model.GetBoardAtFace(rotation));
    }
}

