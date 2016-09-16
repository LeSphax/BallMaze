using BallMaze;
using BallMaze.Cube;
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
    public CameraController cameraController;

    public GameObject levelPrefab;
    private InputManager inputManager;

    public event EmptyEventHandler LevelCompleted;

    void Awake()
    {
        model.HasChanged += RefreshView;
        model.LevelCompleted += RaiseLevelCompleted;
    }

    void Start()
    {
        inputManager = GameObjects.GetInputManager();
        cameraController.PerspectiveActivated += DestroySlice;
        cameraController.SliceChanged += CreateSlice;
    }

    public void CreateSlice(Vector3 rotation)
    {
        currentSlice = Instantiate(levelPrefab).GetComponent<SliceBoard>();
        currentSlice.transform.SetParent(slices.transform, false);
        currentSlice.NotifyObjectivesFilled += model.ObjectivesFilledNotification;

        model.SetSliceBoard(ref currentSlice, rotation);

        inputManager.SetBoard(currentSlice);
        currentView.GetComponent<GetComponentsFadeAnimation>().StartReverseAnimating();
        currentSlice.GetComponent<GetComponentsFadeAnimation>().StartAnimating();
    }

    public void DestroySlice()
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

    public void SetData(CubeData data, bool cameraInit = true)
    {
        if (currentSlice != null)
            DestroyCurrentSlice(this);
        model.SetData(data);
        if (cameraInit)
            cameraController.Init();
    }

    public void RaiseLevelCompleted()
    {
        if (!GameObjects.GetGameState().LevelEditor)
            LevelCompleted.Invoke();
    }
}

