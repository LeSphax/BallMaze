using BallMaze.GameMechanics;
using BallMaze.Saving;
using System.Xml.Serialization;
using UnityEngine;

namespace BallMaze.Inputs
{

    [XmlInclude(typeof(ResetCommand))]
    [XmlInclude(typeof(MoveCommand))]
    [XmlInclude(typeof(CancelCommand))]
    public abstract class BoardInputCommand : InputCommand
    {

        protected PlayBoardModel model;

        public BoardInputCommand()
        {

        }

        public BoardInputCommand(SaveManager saveManager) : base(saveManager)
        {
        }

        protected override void PrepareExecution()
        {
            model = GameObject.FindGameObjectWithTag(Tags.LevelController).GetComponent<PlayBoardModel>();
        }

        public void SetModel(PlayBoardModel model)
        {
            this.model = model;
        }

        public override void LogExecute()
        {
            PrepareExecution();
            saveManager = null;
            model.ReceiveInputCommand(this);
        }

        //public abstract void Undo();

        //public abstract bool IsExecuting();
    }
}
