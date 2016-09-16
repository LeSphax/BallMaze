using BallMaze.Data;
using BallMaze.Extensions;
using BallMaze.GameManagement;
using BallMaze.GameMechanics;
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
        if (!CreatorMode)
        {
            string firstLevelName;
            if (TwoDimensions)
                firstLevelName = Settings.GetFirst2DLevelName();
            else
                firstLevelName = Settings.GetFirst3DLevelName();
            LoadLevel(firstLevelName);
        }
    }

    public bool LoadLevel(string levelName)
    {
        Debug.Log("LoadLevel");
        LevelData newData;
        if (LevelData.TryLoad(levelName, out newData))
        {
            currentData = newData;
            levelNameField.text = currentData.Name;
            SetData(currentData);
        }
        else
        {
            Debug.LogError("Didn't succed in loading the following level : " + levelName);
            return false;
        }
        if (LevelChanged != null)
            LevelChanged.Invoke();
        return true;
    }

    public void SetData(LevelData levelData)
    {
        if (levelData is BoardLevelData)
        {
            SetData(((BoardLevelData)levelData).data);
        }
        else if (levelData is CubeLevelData)
        {
            SetData(((CubeLevelData)levelData).data);
        }
        else
        {
            Debug.LogError("The levelData shouldn't have this type :" + levelData);
        }
    }

    public void SetData(BoardData data)
    {
        Destroy(currentLevel);
        currentLevel = this.InstantiateAsChildren(boardLevelPrefab);
        Board boardModel = currentLevel.GetComponent<Board>();
        boardModel.SetData(data);
        LevelManager levelManager = currentLevel.GetComponent<LevelManager>();
        levelManager.SetObjectiveOrder(currentData.firstObjective);
    }

    public void SetData(CubeData data)
    {
        cubeController.gameObject.SetActive(true);
        cubeController.LevelCompleted -= LoadNextLevelDelayed;
        cubeController.LevelCompleted += LoadNextLevelDelayed;
        cubeController.SetData(data);
    }

    public void LoadNextLevelDelayed()
    {
        Invoke("LoadNextLevel", 0.5f);
    }

    public void LoadPreviousLevel()
    {
        LoadLevel(Levels.GetPreviousLevelName(currentData.fileName));
    }

    public void LoadNextLevel()
    {
        string nextLevelName = Levels.GetPreviousLevelName(currentData.fileName);
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

