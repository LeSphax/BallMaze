



using UnityEngine;
using UnityEngine.UI;


public class LevelLoader : MonoBehaviour
{
    public GameObject boardLevelPrefab;
    public GameObject cubeLevelPrefab;
    public GameObject endOfGameScreen;
    public Text levelNameField;

    private GameObject currentLevel;
    private LevelData currentData;

    public CubeController cubeController;

    public bool CreatorMode;
    public bool TwoDimensions;

    public event EmptyEventHandler LevelChanged;


    void Start()
    {
        InputManager IM = GameObjects.GetInputManager();
        IM.LoadPreviousLevel += LoadPreviousLevel;
        IM.LoadNextLevel += LoadNextLevel;
        IM.ReloadLevel += ReloadLevel;

        if (!CreatorMode)
        {
            string firstLevelName;
            if (TwoDimensions)
                firstLevelName = Levels.GetFirst2DLevelName();
            else
                firstLevelName = Levels.GetFirst3DLevelName();
            LoadLevel(firstLevelName);
        }
    }

    public bool LoadLevel(string levelName)
    {
        //bool loaded;
        bool isCube = Levels.is3DLevel(levelName);
        currentData = LevelData.Load(levelName);
        levelNameField.text = currentData.Name;
        if (isCube)
            SetCubeData(currentData);
        else
            SetBoardData(currentData);
        //if (!loaded)
        //{
        //    Debug.LogError("Didn't succed in loading the following level : " + levelName);
        //    return false;
        //}
        if (LevelChanged != null)
            LevelChanged.Invoke();
        return true;
    }

    public void SetBoardData(LevelData levelData)
    {
        Destroy(currentLevel);
        currentLevel = this.InstantiateAsChildren(boardLevelPrefab);
        Board boardModel = currentLevel.GetComponent<Board>();
        boardModel.SetData((BoardData)levelData.PuzzleData);
        NormalLevelManager levelManager = currentLevel.GetComponent<NormalLevelManager>();
        Debug.Log(currentLevel.name);
        levelManager.SetObjectiveOrder(currentData.firstObjective);
    }

    public void SetCubeData(LevelData levelData)
    {
        cubeController.gameObject.SetActive(true);
        cubeController.LevelCompleted -= LoadNextLevelDelayed;
        cubeController.LevelCompleted += LoadNextLevelDelayed;
        cubeController.SetData((CubeData)levelData.PuzzleData);
    }

    public void LoadNextLevelDelayed()
    {
        Invoke("LoadNextLevel", 0.5f);
    }

    public void LoadPreviousLevel()
    {
        string previousLevel = Levels.GetPreviousLevelName(currentData.FileName);
        if (previousLevel != null)
            LoadLevel(previousLevel);
    }

    public void ReloadLevel()
    {
        Debug.Log("Reload");
        LoadLevel(currentData.FileName);
    }

    public void LoadNextLevel()
    {
        string nextLevelName = Levels.GetNextLevelName(currentData.FileName);
        Debug.Log(nextLevelName);
        if (nextLevelName == null)
            EndOfGame();
        else
            LoadLevel(nextLevelName);
    }

    private void EndOfGame()
    {
        endOfGameScreen.SetActive(true);
    }
}

