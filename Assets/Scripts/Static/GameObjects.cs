using System;
using BallMaze.GameManagement;
using BallMaze.Inputs;
using UnityEngine;
using BallMaze.LevelCreation;

namespace BallMaze
{
    public static class GameObjects
    {
        public static InputManager GetInputManager()
        {
            return GetTaggedComponent<InputManager>(Tags.InputManager);
        }

        public static LevelLoader GetLevelLoader()
        {
            return GetTaggedComponent<LevelLoader>(Tags.BallMazeController);
        }

        private static Type GetTaggedComponent<Type>(string tag)
        {
            GameObject go = GameObject.FindGameObjectWithTag(tag);
            if (go != null)
                return go.GetComponent<Type>();
            return default(Type);
        }

        public static GameState GetGameState()
        {
            return GetTaggedComponent<GameState>(Tags.BallMazeController);
        }

        public static LevelCreatorController GetLevelCreatorController()
        {
            return GetTaggedComponent<LevelCreatorController>(Tags.EditorController);
        }
    }
}

