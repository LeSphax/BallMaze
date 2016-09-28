
using UnityEngine;

namespace BallMaze
{
    class Tags
    {
        public const string LevelController = "LevelController";
        public const string GameController = "GameController";
        public const string BallMazeController = "BallMazeController";
        public const string ResetButton = "ResetButton";
        public const string UndoButton = "UndoButton";
        public const string QuitButton = "QuitButton";
        public const string PreviousLevelButton = "PreviousLevelButton";
        public const string Grid = "Grid";
        public const string SyncedTile = "SyncedTile";
        public const string ObjectiveTile = "ObjectiveTile";

        public const string GameData = "GameData";
        public const string WorldController = "Player";

        public const string CameraController = "CameraController";
        public const string InputManager = "InputManager";

        public const string EditorController = "EditorController";

        public static GameObject[] FindObjectiveTiles()
        {
            var x = GameObject.FindGameObjectsWithTag(SyncedTile);
            var y = GameObject.FindGameObjectsWithTag(ObjectiveTile);
            GameObject[] z = new GameObject[x.Length + y.Length];
            x.CopyTo(z, 0);
            y.CopyTo(z, x.Length);
            return z;
        }
    }
}
