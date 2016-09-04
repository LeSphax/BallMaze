using BallMaze.GameManagement;
using UnityEngine;

namespace BallMaze.Inputs
{
    public class CancelCommand : BoardInputCommand
    {

        public CancelCommand() : base()
        {
        }

        public CancelCommand(SaveManager saveManager) : base(saveManager)
        {
        }

        public override void Execute()
        {
            base.Execute();
            model.CancelLastMovement();
        }

        public override void LogExecute()
        {
            base.LogExecute();
            GameObject.FindGameObjectWithTag(Tags.UndoButton).GetComponent<BlinkingButton>().BlinkOnce();
        }
    }
}