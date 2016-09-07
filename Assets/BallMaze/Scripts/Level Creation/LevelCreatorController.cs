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

        public GameObject ballMaze;
        private LevelLoader levelLoader;

        private GameObject grid;


        private enum State
        {
            CREATE_GRID,
            EDIT_GRID,
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
                    case State.CREATE_GRID:
                        boardData.DestroyBoard();
                        gridController.SetVisible(true);
                        break;
                    case State.EDIT_GRID:
                        gridController.SetVisible(false);
                        ballMaze.SetActive(false);
                        grid.SetActive(true);
                        break;
                    case State.PLAY_TEST:
                        grid.SetActive(false);
                        ballMaze.SetActive(true);
                        break;
                }
            }
        }

        public const string TEMP_LEVEL_NAME = "Temp File";

        private EditableBoardData boardData;
        private GridController gridController;

        void Start()
        {
            grid = GameObject.FindGameObjectWithTag(Tags.Grid);
            gridController = grid.GetComponent<GridController>();
            boardData = new EditableBoardData(grid.GetComponent<EditorBoard>());
            levelLoader = ballMaze.GetComponentInChildren<LevelLoader>();
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt)){
                if (Input.GetKeyDown(KeyCode.S))
                {
                    SaveData();
                }
                if (Input.GetKeyDown(KeyCode.B))
                {
                    state = State.CREATE_GRID;
                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    boardData.ResetBoard();
                    state = State.CREATE_GRID;
                }
            }
        }

        public void PlayTestButtonHandler()
        {
            switch (state)
            {
                case State.CREATE_GRID:
                    break;
                case State.EDIT_GRID:
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
            state = State.EDIT_GRID;
            LevelData level;
            LevelData.TryLoad(TEMP_LEVEL_NAME, out level);
            Assert.IsTrue(level is BoardLevelData);
            boardData.SetData(((BoardLevelData)level).data);
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
                BoardLevelData levelData = CreateLevelData(levelName);
                if (!levelData.Save(levelName, force))
                {
                    ActivatePopUp(true);
                    return false;
                }
                return true;
            }
        }

        private BoardLevelData CreateLevelData(string levelName)
        {
            BoardLevelData levelData = new BoardLevelData(boardData, previousLevelNameField.text, levelName, nextLevelNameField.text);
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
                state = State.EDIT_GRID;
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
                Assert.IsTrue(level is BoardLevelData);
                boardData.SetData(((BoardLevelData)level).data);
            }

        }

        private void SaveDataForPlay()
        {
            CreateLevelData(TEMP_LEVEL_NAME).Save(TEMP_LEVEL_NAME, true);
        }

        public void OnGridEnter(int x, int y)
        {
            switch (state)
            {
                case State.CREATE_GRID:
                    boardData.CreateBoard(x + 1, y + 1);
                    break;
                case State.EDIT_GRID:
                    break;
            }
        }

        public void OnGridClick(int x, int y)
        {
            switch (state)
            {
                case State.CREATE_GRID:
                    state = State.EDIT_GRID;
                    break;
                case State.EDIT_GRID:
                    if (x < boardData.Width && y < boardData.Height)
                    {
                        MapClicksToCreation(x, y);
                    }
                    break;
            }
        }

        private void MapClicksToCreation(int x, int y)
        {
            if (Input.GetMouseButton(0))
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    boardData.NextBall(x, y);
                }
                else if (Input.GetKey(KeyCode.LeftAlt))
                {
                    boardData.NextTileType(x, y);
                }
                else
                {
                    boardData.NextTileObjective(x, y);
                }
            }
            else if (Input.GetMouseButton(1))
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    boardData.PreviousBall(x, y);
                }
                else if (Input.GetKey(KeyCode.LeftAlt))
                {
                    boardData.PreviousTileType(x, y);
                }
                else
                {
                    boardData.PreviousTileObjective(x, y);
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
