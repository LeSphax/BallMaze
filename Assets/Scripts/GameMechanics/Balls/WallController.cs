using System;

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
