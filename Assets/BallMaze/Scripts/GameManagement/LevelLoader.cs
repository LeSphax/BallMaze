using BallMaze.Data;
using BallMaze.Extensions;
using BallMaze.GameMechanics;
using BallMaze.Inputs;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace BallMaze.GameManagement
{

    public class LevelLoader : MonoBehaviour
    {
        public string firstLevelName;
        public GameObject boardLevelPrefab;
        public GameObject cubeLevelPrefab;
        public GameObject endOfGameScreen;
        public Text levelNameField;

        private GameObject currentLevel;
        private LevelData currentData;

        private CubeController cubeController;

        public bool CreatorMode;

        public event EmptyEventHandler LevelChanged;


        void Awake()
        {
            if (!CreatorMode)
                LoadLevel(firstLevelName);
        }

        public void LoadLevel(string levelName)
        {
            if (LevelData.TryLoad(levelName, out currentData))
            {
                levelNameField.text = currentData.name;
                SetData(currentData);
            }
            else
            {
                Debug.LogError("Didn't succed in loading the following level : " + levelName);
            }
            if (LevelChanged != null)
                LevelChanged.Invoke();
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
            if (currentLevel == null)
            {
                currentLevel = this.InstantiateAsChildren(cubeLevelPrefab);
                cubeController = currentLevel.GetComponent<CubeController>();
            }
            cubeController.SetData(data);
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
