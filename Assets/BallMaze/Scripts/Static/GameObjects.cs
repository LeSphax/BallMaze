using BallMaze.Inputs;
using UnityEngine;

namespace BallMaze
{
    public static class GameObjects
    {
        public static InputManager GetInputManager()
        {
            if (GameObject.FindGameObjectWithTag(Tags.InputManager) != null)
                return GameObject.FindGameObjectWithTag(Tags.InputManager).GetComponent<InputManager>();
            return null;
        }
    }
}

