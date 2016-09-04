namespace BallMaze.GameMechanics.Commands
{
    public delegate void BallCommandEventHandler(AbstractBallCommand sender);



    public abstract class AbstractBallCommand
    {
        public event BallCommandEventHandler FinishedExecuting;

        public abstract void Execute();
        public virtual void Undo()
        {
            if (WasUseful())
            {
                ExecuteUndo();
            }
            else
            {
                RaiseFinishedExecuting();
            }
        }

        protected abstract void ExecuteUndo();

        protected void RaiseFinishedExecuting()
        {
            TakeOffListener();
            if (FinishedExecuting != null)
            {
                FinishedExecuting.Invoke(this);
            }
        }

        protected abstract void TakeOffListener();

        public virtual bool WasUseful()
        {
            return true;
        }
    }
}