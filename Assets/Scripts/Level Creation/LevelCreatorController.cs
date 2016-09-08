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

        public InputField previousLevelNameField;
        public InputField levelNameField;
        public InputField nextLevelNameField;
        public InputField numberMovesField;

        public Dropdown FirstObjective;
        public Dropdown SecondObjective;

        public GameObject PopUp;
        public CubeController cubeController;

        private LevelLoader levelLoader;

        private GameObject grid;


        private enum State
        {
            CREATE_CUBE,
            EDIT_CUBE,
            PLAY_TEST,
        }
        private State _state;
        private State state
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                switch (_state)
                {
                    case State.CREATE_CUBE:
                        boardData.DestroyBoard();
                        gridController.SetVisible(true);
                        break;
                    case State.EDIT_CUBE:
                        gridController.SetVisible(false);
                        //ballMaze.SetActive(false);
                        grid.SetActive(true);
                        break;
                    case State.PLAY_TEST:
                        grid.SetActive(false);
                        //ballMaze.SetActive(true);
                        break;
                }
            }
        }

        public const string TEMP_LEVEL_NAME = "Temp File";

        private EditableCubeData boardData;
        private GridController gridController;

        void Start()
        {
            grid = GameObject.FindGameObjectWithTag(Tags.Grid);
            gridController = grid.GetComponent<GridController>();
            boardData = new EditableCubeData(cubeController);
            levelLoader = GameObjects.GetLevelLoader();
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt))
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    SaveData();
                }
                if (Input.GetKeyDown(KeyCode.B))
                {
                    state = State.CREATE_CUBE;
                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    boardData.ResetBoard();
                    state = State.CREATE_CUBE;
                }
            }
        }

        public void PlayTestButtonHandler()
        {
            switch (state)
            {
                case State.CREATE_CUBE:
                    break;
                case State.EDIT_CUBE:
                    StartPlayTest();
                    break;
                case State.PLAY_TEST:
                    StopPlayTest();
                    break;
            }
        }

        private void StartPlayTest()
        {
            state = State.PLAY_TEST;
            SaveDataForPlay();
            levelLoader.LoadLevel(TEMP_LEVEL_NAME);
        }

        private void StopPlayTest()
        {
            state = State.EDIT_CUBE;
            LevelData level;
            LevelData.TryLoad(TEMP_LEVEL_NAME, out level);
            Assert.IsTrue(level is CubeLevelData);
            boardData.SetData(((CubeLevelData)level).data);
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
                state = State.EDIT_CUBE;
                previousLevelNameField.text = level.previousLevelName;
                levelNameField.text = level.Name;
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

        //public void OnGridClick(int x, int y)
        //{
        //    switch (state)
        //    {
        //        case State.CREATE_CUBE:
        //            state = State.EDIT_CUBE;
        //            break;
        //        case State.EDIT_CUBE:
        //            if (x < boardData.X_SIZE && y < boardData.Y_SIZE)
        //            {
        //                MapClicksToCreation(x, y);
        //            }
        //            break;
        //    }
        //}

        //private void MapClicksToCreation(int x, int y)
        //{
        //    if (Input.GetMouseButton(0))
        //    {
        //        if (Input.GetKey(KeyCode.LeftControl))
        //        {
        //            boardData.NextBall(x, y);
        //        }
        //        else if (Input.GetKey(KeyCode.LeftAlt))
        //        {
        //            boardData.NextTileType(x, y);
        //        }
        //        else
        //        {
        //            boardData.NextTileObjective(x, y);
        //        }
        //    }
        //    else if (Input.GetMouseButton(1))
        //    {
        //        if (Input.GetKey(KeyCode.LeftControl))
        //        {
        //            boardData.PreviousBall(x, y);
        //        }
        //        else if (Input.GetKey(KeyCode.LeftAlt))
        //        {
        //            boardData.PreviousTileType(x, y);
        //        }
        //        else
        //        {
        //            boardData.PreviousTileObjective(x, y);
        //        }
        //    }
        //}

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
