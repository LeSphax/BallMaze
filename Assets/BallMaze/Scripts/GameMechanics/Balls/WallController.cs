namespace BallMaze.GameMechanics.Balls
{
    class WallController : BallController<BallView>
    {
        public override bool IsWall()
        {
            return true;
        }
    }
}
