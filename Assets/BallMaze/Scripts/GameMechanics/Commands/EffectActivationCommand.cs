
using BallMaze.GameMechanics.Tiles;

namespace BallMaze.GameMechanics.Commands
{
    internal class EffectActivationCommand : AbstractBallCommand
    {
        IBallModel ball;
        TileModel tile;
        bool wasUseful;
        bool previousActivation;

        public EffectActivationCommand(IBallModel ball, TileModel tile)
        {
            this.ball = ball;
            this.tile = tile;
        }

        public override void Execute()
        {
            previousActivation = tile.IsEffectActivated();
            wasUseful = tile.ActivateEffect(ball);
            RaiseFinishedExecuting();
        }

        protected override void ExecuteUndo()
        {
            tile.ActivateEffect(!previousActivation);
            RaiseFinishedExecuting();
        }

        protected override void TakeOffListener()
        {
        }

        public override bool WasUseful()
        {
            return wasUseful;
        }
    }
}