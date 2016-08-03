using BallMaze.GameManagement;
using BallMaze.Saving;
using UnityEngine;

namespace BallMaze.Inputs
{
    public class PreviousLevelCommand : InputCommand
    {

        LevelLoader loader;

        public PreviousLevelCommand()
        {

        }

        public PreviousLevelCommand(SaveManager saveManager, LevelLoader loader) : base(saveManager)
        {
            this.loader = loader;
        }

        public override void Execute()
        {
            base.Execute();
            loader.LoadPreviousLevel();
        }

        protected override void PrepareExecution()
        {
            loader = GameObject.FindGameObjectWithTag(Tags.BallMazeController).GetComponent<LevelLoader>();
        }

        public override void LogExecute()
        {
            base.LogExecute();
            GameObject.FindGameObjectWithTag(Tags.PreviousLevelButton).GetComponent<BlinkingButton>().BlinkOnce();
        }
    }
}