using BallMaze.GameManagement;
using BallMaze.Inputs;
using UnityEngine;

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
    }
}

