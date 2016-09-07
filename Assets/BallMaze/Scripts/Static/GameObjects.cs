using BallMaze.GameManagement;
using BallMaze.Inputs;
using UnityEngine;

namespace BallMaze
{
    public static class GameObjects
    {
        public static PCInputManager GetInputManager()
        {
            return GetTaggedComponent<PCInputManager>(Tags.InputManager);
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

