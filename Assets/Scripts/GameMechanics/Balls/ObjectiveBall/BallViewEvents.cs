using BallMaze.GameMechanics.Tiles;
using UnityEngine;

namespace BallMaze.GameMechanics.ObjectiveBall
{
    internal abstract class BallViewEvent
    {
    }

    internal class MoveCommand : BallViewEvent
    {
        internal Vector3 target;

        public MoveCommand(Vector3 target)
        {
            this.target = target;
        }
    }

    internal class CompleteCommand : BallViewEvent
    {
        internal TileController objective;
        

        public CompleteCommand(TileController objective)
        {
            this.objective = objective;
        }
    }

    internal class FinishedAnimation : BallViewEvent
    {
        internal MonoBehaviour animation;

        internal FinishedAnimation(MonoBehaviour animation)
        {
            this.animation = animation;
        }
    }
}
