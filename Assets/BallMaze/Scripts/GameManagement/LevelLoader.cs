using BallMaze.Data;
using BallMaze.Extensions;
using BallMaze.GameMechanics;
using BallMaze.Inputs;
using UnityEngine;
using UnityEngine.UI;

namespace BallMaze.GameManagement
{

    public class LevelLoader : MonoBehaviour
    {
        public string firstLevelName;
        public GameObject levelPrefab;
        public GameObject endOfGameScreen;
        public Text levelNameField;

        private GameObject currentLevel;
        private LevelData currentData;
        private InputManager inputManager;

        public bool CreatorMode;


        void Awake()
        {
            inputManager = GetComponent<InputManager>();
            //if (NavigationManager.firstLevelName != "")
            //{
            //    firstLevelName = NavigationManager.firstLevelName;
            //}
            if (!CreatorMode)
                LoadLevel(firstLevelName);
        }

        public void LoadLevel(string levelName)
        {
            Destroy(currentLevel);
            if (LevelData.TryLoad(levelName, out currentData))
            {
                levelNameField.text = currentData.name;
                currentLevel = this.InstantiateAsChildren(levelPrefab);
                Board boardModel = currentLevel.GetComponent<Board>();
                boardModel.SetData(currentData.boardData);

                LevelManager levelManager = currentLevel.GetComponent<LevelManager>();
                levelManager.SetObjectiveOrder(currentData.firstObjective);
                inputManager.SetBoard(boardModel);
            }
        }

        public void LoadNextLevel()
        {
            Invoke("_LoadNextLevel", 0.5f);
        }

        public void LoadPreviousLevel()
        {
            if (currentData.HasPreviousLevel())
                LoadLevel(currentData.previousLevelName);
        }

        void _LoadNextLevel()
        {
            if (currentData.HasNextLevel())
            {
                LoadLevel(currentData.nextLevelName);
            }
            else
            {
                if (!CreatorMode)
                EndOfGame();
            }
        }

        private void EndOfGame()
        {
            endOfGameScreen.SetActive(true);
        }
    }
}
