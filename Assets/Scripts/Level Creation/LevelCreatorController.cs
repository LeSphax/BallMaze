
using System.IO;
using UnityEngine;
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

    private LevelData currentData;

    private int currentElevation = 0;

    public string CurrentLevelName
    {
        get
        {
            return levelNameField.text;
        }
    }

    public string NextLevelName
    {
        get
        {
            return Levels.GetNextLevelName(currentData.FileName);
        }
    }

    public string PreviousLevelName
    {
        get
        {
            return Levels.GetPreviousLevelName(currentData.FileName);
        }
    }
    private EditableCubeData boardData;

    void Start()
    {
#if UNITY_EDITOR && ! UNITY_WEBPLAYER
        Levels.WriteLevels();
#endif
        EmptyEventHandler resetElevation = delegate () { currentElevation = 0; };
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


    private void SaveData(bool force = false)
    {
        string levelName = levelNameField.text;
        if (levelName == "")
        {
            Debug.LogWarning("The level needs a name");
            levelNameField.Select();
        }
        else
        {
            currentData = CreateLevelData(levelName);
            FileInfo file = new FileInfo(Application.dataPath + "/Resources/LevelFiles/" + levelName + ".txt");

            if (!force && file.Exists)
            {
                ActivatePopUp(true);
            }
            else
            {
                file.Directory.Create(); // If the directory already exists, this method does nothing.
                File.WriteAllText(file.FullName, currentData.Serialize());
            }
        }
    }

    private LevelData CreateLevelData(string levelName)
    {
        LevelData levelData = new LevelData(boardData, levelName);
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
        currentData = LevelData.Load(levelName);
        levelNameField.text = currentData.FileName;
        previousLevelNameField.text = PreviousLevelName;
        nextLevelNameField.text = NextLevelName;
        numberMovesField.text = currentData.numberMoves.ToString();
        switch (currentData.firstObjective)
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
        boardData.SetData((CubeData)currentData.PuzzleData);
    }

    public void OnTileClick(CubeFace face, int x, int y)
    {
        FaceModel faceModel = FaceModel.ModelsDictionary[face];
        int Z_SIZE = faceModel.GetRealSizes(boardData.Sizes)[Axis.Z];
        IntVector3 position = faceModel.GetRealCoords(new IntVector3(x, y, Z_SIZE - 1 - currentElevation), boardData.Sizes);
        if (position.X < boardData.X_SIZE && position.Y < boardData.Y_SIZE && position.Z < boardData.Z_SIZE)
        {
            MapClicksToCreation(face, x, y, position);
        }
        else
        {
            Debug.LogWarning("OutOfCube");
            Debug.Log(position.X + " vs " + boardData.X_SIZE);
            Debug.Log(position.Y + " vs " + boardData.Y_SIZE);
            Debug.Log(position.Z + " vs " + boardData.Z_SIZE);
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
        LoadLevel(PreviousLevelName);
    }

    public void LoadCurrent()
    {
        LoadLevel(CurrentLevelName);
    }

    public void LoadNext()
    {
        LoadLevel(NextLevelName);
    }
}
