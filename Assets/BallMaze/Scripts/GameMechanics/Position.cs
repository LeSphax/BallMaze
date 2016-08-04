
using BallMaze.GameMechanics.Tiles;

namespace BallMaze.GameMechanics
{
    public class Position
    {
        internal TileController tile;
        internal IBallController ball;

        public Position()
        {

        }

        internal Position(TileController tile, IBallController ball)
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
