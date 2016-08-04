using System;
using System.Xml.Serialization;
using BallMaze.Saving;

namespace BallMaze.Inputs
{
    [Serializable]
    [XmlInclude(typeof(BoardInputCommand))]
   // [XmlInclude(typeof(QuitCommand))]
    [XmlInclude(typeof(PreviousLevelCommand))]
    public abstract class InputCommand
    {

        protected SaveManager saveManager;
        private SaveManager saveManager1;

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
