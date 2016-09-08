using BallMaze.Inputs;
using System;
using UnityEngine;

namespace BallMaze.GameMechanics.Commands
{
    [Serializable]
    internal class BallMoveCommand : AbstractBallCommand
    {
        private Direction direction;

        private IBallController ball;

        private Vector2 oldBallPosition;
        private Vector2 newBallPosition;

        public BallMoveCommand(IBallController ball, Direction direction)
        {
            this.ball = ball;
            this.direction = direction;
        }

        public override void Execute()
        {
            ball.FinishedAnimating += new EmptyEventHandler(RaiseFinishedExecuting);
            oldBallPosition = ball.GetPosition();
            ball.Move(direction);
            newBallPosition = ball.GetPosition();
        }

        protected override void ExecuteUndo()
        {
            ball.FinishedAnimating += new EmptyEventHandler(RaiseFinishedExecuting);
            ball.MoveBack((int)oldBallPosition.x, (int)oldBallPosition.y);
        }

        protected override void TakeOffListener()
        {
            ball.FinishedAnimating -= new EmptyEventHandler(RaiseFinishedExecuting);
        }

        public override bool WasUseful()
        {
            return oldBallPosition != newBallPosition;
        }
    }
}

