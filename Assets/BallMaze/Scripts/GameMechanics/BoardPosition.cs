
using BallMaze.GameMechanics.Tiles;

namespace BallMaze.GameMechanics
{
    public class BoardPosition
    {
        internal TileModel tile;
        internal IBallModel ball;

        public BoardPosition()
        {

        }

        internal BoardPosition(TileModel tile, IBallModel ball)
        {
            this.tile = tile;
            this.ball = ball;
        }

        public override string ToString()
        {
            if (ball != null)
                return ball.ToString();
            else
            {
                return "null";
            }
        }
    }
}
