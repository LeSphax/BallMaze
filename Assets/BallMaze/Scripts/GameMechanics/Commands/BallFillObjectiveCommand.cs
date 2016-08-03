using BallMaze.GameManagement;
using BallMaze.GameMechanics.Tiles;
using UnityEngine;

namespace BallMaze.GameMechanics.Commands
{
    internal class BallFillObjectiveCommand : AbstractBallCommand
    {

        private IBallModel ball;
        private TileModel tile;
        private bool wasUseful = false;

        public BallFillObjectiveCommand(IBallModel ball, TileModel tile)
        {
            this.ball = ball;
            this.tile = tile;
        }

        public override void Execute()
        {
            if (tile.TryFillTile())
            {
                ball.FinishedAnimating += new EmptyEventHandler(RaiseFinishedExecuting);
                GameObject.FindGameObjectWithTag(Tags.LevelController).GetComponent<LevelManager>().NotifyFilledObjective(tile.GetObjectiveType());
                ball.FillObjective();
                wasUseful = true;
            }
            else
            {
                RaiseFinishedExecuting();
            }
        }

        protected override void ExecuteUndo()
        {
            ball.FinishedAnimating += new EmptyEventHandler(RaiseFinishedExecuting);
            tile.UnFillTile();
            ball.UnFillObjective();
            GameObject.FindGameObjectWithTag(Tags.LevelController).GetComponent<LevelManager>().NotifyUnFilledObjective(ball.GetObjectiveType());
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

