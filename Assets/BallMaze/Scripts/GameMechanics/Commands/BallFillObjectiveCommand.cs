using BallMaze.GameManagement;
using BallMaze.GameMechanics.Tiles;
using UnityEngine;

namespace BallMaze.GameMechanics.Commands
{
    internal class BallFillObjectiveCommand : AbstractBallCommand
    {

        private IBallController ball;
        private TileController tile;
        private bool wasUseful = false;

        public BallFillObjectiveCommand(IBallController ball, TileController tile)
        {
            this.ball = ball;
            this.tile = tile;
        }

        public override void Execute()
        {
            if (tile.TryFillTile())
            {
                wasUseful = true;
                ball.FinishedAnimating += new EmptyEventHandler(RaiseFinishedExecuting);
                GameObject.FindGameObjectWithTag(Tags.LevelController).GetComponent<LevelManager>().NotifyFilledObjective(tile.GetObjectiveType());
                ball.FillObjective();
            }
            else
            {
                RaiseFinishedExecuting();
            }
        }

        protected override void ExecuteUndo()
        {
            ball.FinishedAnimating += new EmptyEventHandler(RaiseFinishedExecuting);
            GameObject.FindGameObjectWithTag(Tags.LevelController).GetComponent<LevelManager>().NotifyUnFilledObjective(ball.GetObjectiveType());
            tile.UnFillTile();
            ball.UnFillObjective();
        }

        protected override void TakeOffListener()
        {
            ball.FinishedAnimating -= new EmptyEventHandler(RaiseFinishedExecuting);
        }

        public override bool WasUseful()
        {
            return wasUseful;
        }
    }
}

