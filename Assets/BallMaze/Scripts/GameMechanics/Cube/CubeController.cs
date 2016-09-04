using BallMaze;
using BallMaze.Data;
using BallMaze.Extensions;
using BallMaze.GameMechanics;
using BallMaze.Inputs;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public GameObject CubeViewPrefab;
    private GameObject currentView;

    private SliceBoard currentSlice;
    public GameObject slices;
    public GameObject cubeView;

    public CubeModel model;

    public GameObject levelPrefab;
    private InputManager inputManager;

    void Awake()
    {
        model.HasChanged += RefreshView;
    }

    void Start()
    {
        inputManager = GameObjects.GetInputManager();
        model.SetDummyCubeModel();
        CameraTurnAround cameraController = GameObject.FindGameObjectWithTag(Tags.CameraController).GetComponent<CameraTurnAround>();
        cameraController.RotationChangeStart += DestroySlice;
        cameraController.RotationChanged += CreateSlice;
        cameraController.Init();
    }

    public void CreateSlice(Vector3 rotation)
    {
        currentSlice = Instantiate(levelPrefab).GetComponent<SliceBoard>();
        currentSlice.transform.SetParent(slices.transform, false);

        model.SetSliceBoard(ref currentSlice, rotation);

        inputManager.SetBoard(currentSlice);
        currentView.GetComponent<GetComponentsFadeAnimation>().StartReverseAnimating();
        currentSlice.GetComponent<GetComponentsFadeAnimation>().StartAnimating();
    }

    public void DestroySlice(Vector3 rotation)
    {
        if (currentSlice != null)
        {
            model.SetNewBallPositions(currentSlice);
            currentSlice.GetComponent<GetComponentsFadeAnimation>().FinishedAnimating += DestroyCurrentSlice;
            currentView.GetComponent<GetComponentsFadeAnimation>().StartAnimating();
            currentSlice.GetComponent<GetComponentsFadeAnimation>().StartReverseAnimating();
        }
    }

    private void DestroyCurrentSlice(MonoBehaviour sender)
    {
        Debug.Log("Destroy");
        Destroy(currentSlice.gameObject);
    }

    public void RefreshView(CubeModel model)
    {
        if (currentView != null)
        {
            Destroy(currentView);
        }
        currentView = Instantiate(CubeViewPrefab);
        currentView.transform.SetParent(cubeView.transform, false);
        currentView.GetComponent<CubeView>().RefreshView(model);
    }

    public void SetData(CubeData data)
    {
        model.SetData(data);
    }
}

