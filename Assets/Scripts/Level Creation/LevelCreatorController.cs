using BallMaze.Cube;
using BallMaze.Data;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;


public class LevelCreatorController : MonoBehaviour
{
    public bool autoLoad;
    public InputField previousLevelNameField;
    public InputField levelNameField;
    public InputField nextLevelNameField;
    public InputField numberMovesField;

    public Dropdown FirstObjective;
    public Dropdown SecondObjective;

    public GameObject PopUp;
    public CubeController cubeController;

    private int currentElevation = 0;

    public string currentLevelName
    {
        get
        {
            return levelNameField.text;
        }
    }

    public string nextLevelName
    {
        get
        {
            return Levels.GetNextLevelName(currentLevelName);
        }
    }

    public string previousLevelName
    {
        get
        {
            return Levels.GetPreviousLevelName(currentLevelName);
        }
    }
    private EditableCubeData boardData;

    void Start()
    {
        Levels.WriteLevels();
        EmptyEventHandler resetElevation = delegate () { Debug.Log("reset"); currentElevation = 0; };
        GameObjects.GetCameraController().RotationChanged += resetElevation;
        boardData = new EditableCubeData(cubeController);
        boardData.CreateBoard(new IntVector3(3, 3, 3));
        if (autoLoad)
        {
            LoadLevel(Levels.GetFirst3DLevelName());
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt))
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                SaveData();
            }
            else if (Input.GetKeyDown(KeyCode.B))
            {
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                boardData.ResetBoard();
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                LoadPrevious();
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                LoadCurrent();
            }
            else if (Input.GetKeyDown(KeyCode.M))
            {
                LoadNext();
            }
        }
        else if (Input.GetKeyDown(KeyCode.A))
            currentElevation = Functions.mod(currentElevation - 1, 3);
        else if (Input.GetKeyDown(KeyCode.E))
            currentElevation = Functions.mod(currentElevation + 1, 3);
    }

    private void ActivatePopUp(bool open)
    {
        PopUp.SetActive(open);
    }

    public void ReplaceButtonHandler()
    {
        SaveData(true);
        ActivatePopUp(false);
    }


    private bool SaveData(bool force = false)
    {
        string levelName = levelNameField.text;
        if (levelName == "")
        {
            Debug.LogWarning("The level needs a name");
            levelNameField.Select();
            return false;
        }
        else
        {
            CubeLevelData levelData = CreateLevelData(levelName);
            if (!levelData.Save(levelName, force))
            {
                ActivatePopUp(true);
                return false;
            }
            return true;
        }
    }

    private CubeLevelData CreateLevelData(string levelName)
    {
        CubeLevelData levelData = new CubeLevelData(boardData, previousLevelNameField.text, levelName, nextLevelNameField.text);
        if (FirstObjective.value == 1)
        {
            levelData.SetFirstObjective(ObjectiveType.OBJECTIVE1);
        }
        else if (FirstObjective.value == 2)
        {
            levelData.SetFirstObjective(ObjectiveType.OBJECTIVE2);
        }
        int numberMoves;
        if (int.TryParse(numberMovesField.text, out numberMoves))
            levelData.SetNumberMoves(numberMoves);
        return levelData;
    }

    private void LoadLevel(string levelName)
    {
        LevelData level;
        if (LevelData.TryLoad(levelName, out level))
        {
            levelNameField.text = level.fileName;
            previousLevelNameField.text = previousLevelName;
            nextLevelNameField.text = nextLevelName;
            numberMovesField.text = level.numberMoves.ToString();
            switch (level.firstObjective)
            {
                case ObjectiveType.NONE:
                    FirstObjective.value = 0;
                    break;
                case ObjectiveType.OBJECTIVE1:
                    FirstObjective.value = 1;
                    break;
                case ObjectiveType.OBJECTIVE2:
                    FirstObjective.value = 2;
                    break;
            }
            Assert.IsTrue(level is CubeLevelData);
            boardData.SetData(((CubeLevelData)level).data);
        }

    }

    public void OnTileClick(CubeFace face, int x, int y)
    {
        FaceModel faceModel = FaceModel.ModelsDictionary[face];
        int Z_SIZE = faceModel.GetRealSizes(boardData.Sizes)[Axis.Z];
        IntVector3 position = faceModel.GetRealCoords(new IntVector3(x, y, Z_SIZE - 1 - currentElevation), boardData.Sizes);
        if (position.x < boardData.X_SIZE && position.y < boardData.Y_SIZE && position.z < boardData.Z_SIZE)
        {
            MapClicksToCreation(face, x, y, position);
        }
        else
        {
            Debug.LogWarning("OutOfCube");
            Debug.Log(position.x + " vs " + boardData.X_SIZE);
            Debug.Log(position.y + " vs " + boardData.Y_SIZE);
            Debug.Log(position.z + " vs " + boardData.Z_SIZE);
        }

    }

    private void MapClicksToCreation(CubeFace face, int x, int y, IntVector3 position)
    {
        if (Input.GetMouseButton(0))
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                boardData.NextTileObjective((int)face, x, y);
            }
            else if (Input.GetKey(KeyCode.LeftAlt))
            {
                boardData.NextTileType((int)face, x, y);
            }
            else
            {
                boardData.NextBall(position);
            }
        }
        else if (Input.GetMouseButton(1))
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                boardData.PreviousTileObjective((int)face, x, y);
            }
            else if (Input.GetKey(KeyCode.LeftAlt))
            {
                boardData.PreviousTileType((int)face, x, y);
            }
            else
            {
                boardData.PreviousBall(position);
            }
        }
    }

    public void LoadPrevious()
    {
        LoadLevel(previousLevelName);
    }

    public void LoadCurrent()
    {
        LoadLevel(currentLevelName);
    }

    public void LoadNext()
    {
        LoadLevel(nextLevelName);
    }
}
