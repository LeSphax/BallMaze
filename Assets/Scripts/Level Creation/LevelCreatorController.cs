using BallMaze.Cube;
using BallMaze.Data;
using BallMaze.GameManagement;
using BallMaze.LevelCreation.Grid;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace BallMaze.LevelCreation
{
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

        //private State _state;
        //private State state
        //{
        //    get
        //    {
        //        return _state;
        //    }
        //    set
        //    {
        //        _state = value;
        //        switch (_state)
        //        {
        //            case State.CREATE_CUBE:
        //                boardData.DestroyBoard();
        //                break;
        //            case State.EDIT_CUBE:
        //                break;
        //            case State.PLAY_TEST:
        //                break;
        //        }
        //    }
        //}

        public const string TEMP_LEVEL_NAME = "Temp File";

        private EditableCubeData boardData;

        void Start()
        {
            boardData = new EditableCubeData(cubeController);
            boardData.CreateBoard(new IntVector3(3, 3, 3));
            if (autoLoad)
            {
                LoadLevel(Settings.GetFirstLevelName());
            }
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt))
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                   // SaveData();
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
                previousLevelNameField.text = level.previousLevelName;
                levelNameField.text = level.fileName;
                nextLevelNameField.text = level.nextLevelName;
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

        private void SaveDataForPlay()
        {
            CreateLevelData(TEMP_LEVEL_NAME).Save(TEMP_LEVEL_NAME, true);
        }

        //public void OnGridEnter(int x, int y)
        //{
        //    switch (state)
        //    {
        //        case State.CREATE_CUBE:
        //            boardData.CreateBoard(x + 1, y + 1, z + 1);
        //            break;
        //        case State.EDIT_CUBE:
        //            break;
        //    }
        //}

        public void OnTileClick(CubeFace face, int x, int y)
        {
            IntVector3 position = FaceModel.ModelsDictionary[face].GetRealCoords(new IntVector3(x, y, currentElevation), boardData.Sizes);
            if (position.x < boardData.X_SIZE && position.y < boardData.Y_SIZE && position.z < boardData.Z_SIZE)
            {
                MapClicksToCreation(face, x, y, position);
            }
            else
            {
                Debug.LogWarning("OutOfCube");
                Debug.Log(position.x < boardData.X_SIZE);
                Debug.Log(position.y < boardData.Y_SIZE);
                Debug.Log(position.z < boardData.Z_SIZE);
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
            LoadLevel(previousLevelNameField.text);
        }

        public void LoadCurrent()
        {
            LoadLevel(levelNameField.text);
        }

        public void LoadNext()
        {
            LoadLevel(nextLevelNameField.text);
        }
    }
}
