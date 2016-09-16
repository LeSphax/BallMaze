using System;

namespace BallMaze.GameMechanics.Balls
{
    class WallController : BallController<BallView>
    {
        public override bool IsWall()
        {
            return true;
        }

        public override BallType GetBallType()
        {
            return BallType.WALL;
        }
    }
}
