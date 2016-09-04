using BallMaze.GameManagement;
using UnityEngine;

namespace BallMaze.Inputs
{
    public class ResetCommand : BoardInputCommand
    {
        public ResetCommand()
        {

        }
        public ResetCommand(SaveManager saveManager) : base(saveManager)
        {
        }

        public override void Execute()
        {
            base.Execute();
            model.Reset();
        }

        public override void LogExecute()
        {
            base.LogExecute();
            GameObject.FindGameObjectWithTag(Tags.ResetButton).GetComponent<BlinkingButton>().BlinkOnce();
        }
    }
}
