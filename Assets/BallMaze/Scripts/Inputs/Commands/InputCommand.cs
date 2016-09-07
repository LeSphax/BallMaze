using BallMaze.GameManagement;
using System;
using System.Xml.Serialization;

namespace BallMaze.Inputs
{
    [Serializable]
    [XmlInclude(typeof(BoardInputCommand))]
   // [XmlInclude(typeof(QuitCommand))]
    [XmlInclude(typeof(PreviousLevelCommand))]
    public abstract class InputCommand
    {

        protected SaveManager saveManager;

        public InputCommand()
        {

        }

        public InputCommand(SaveManager saveManager)
        {
            this.saveManager = saveManager;
        }

        public virtual void Execute()
        {
            if (saveManager != null) saveManager.AddLog(this);
        }

        public virtual void LogExecute()
        {
            PrepareExecution();
            saveManager = null;
            Execute();
        }

        protected abstract void PrepareExecution();
    }
}
