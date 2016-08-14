using BallMaze.Inputs;
using UnityEngine;

namespace BallMaze
{
    public static class GameObjects
    {
        public static InputManager GetInputManager()
        {
            return GameObject.FindGameObjectWithTag(Tags.InputManager).GetComponent<InputManager>();
        }
    }
}

