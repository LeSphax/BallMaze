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
                firstLevelName = Levels.GetFirst2DLevelName();
            else
                firstLevelName = Levels.GetFirst3DLevelName();
            LoadLevel(firstLevelName);
        }
    }

    public bool LoadLevel(string levelName)
    {
        bool loaded;
        if (Levels.is3DLevel(levelName))
        {
            CubeLevelData newData;
            loaded = LevelData.TryLoad(levelName, out newData);
            if (loaded)
            {
                currentData = newData;
                levelNameField.text = currentData.Name;
                SetData(newData);
            }
        }
        else
        {
            BoardLevelData newData;
            loaded = LevelData.TryLoad(levelName, out newData);
            if (loaded)
            {
                currentData = newData;
                levelNameField.text = currentData.Name;
                SetData(newData);
            }
        }
        if (!loaded)
        {
            Debug.LogError("Didn't succed in loading the following level : " + levelName);
            return false;
        }
        if (LevelChanged != null)
            LevelChanged.Invoke();
        return true;
    }

    public void SetData(BoardLevelData levelData)
    {
        Destroy(currentLevel);
        currentLevel = this.InstantiateAsChildren(boardLevelPrefab);
        Board boardModel = currentLevel.GetComponent<Board>();
        boardModel.SetData(levelData.data);
        NormalLevelManager levelManager = currentLevel.GetComponent<NormalLevelManager>();
        Debug.Log(currentLevel.name);
        levelManager.SetObjectiveOrder(currentData.firstObjective);
    }

    public void SetData(CubeLevelData levelData)
    {
        cubeController.gameObject.SetActive(true);
        cubeController.LevelCompleted -= LoadNextLevelDelayed;
        cubeController.LevelCompleted += LoadNextLevelDelayed;
        cubeController.SetData(levelData.data);
    }

    public void LoadNextLevelDelayed()
    {
        Invoke("LoadNextLevel", 0.5f);
    }

    public void LoadPreviousLevel()
    {
        LoadLevel(Levels.GetPreviousLevelName(currentData.FileName));
    }

    public void LoadNextLevel()
    {
        string nextLevelName = Levels.GetNextLevelName(currentData.FileName);
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

