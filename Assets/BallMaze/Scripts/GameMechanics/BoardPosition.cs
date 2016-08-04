
using BallMaze.GameMechanics.Tiles;

namespace BallMaze.GameMechanics
{
    public class BoardPosition
    {
        internal TileController tile;
        internal IBallController ball;

        public BoardPosition()
        {

        }

        internal BoardPosition(TileController tile, IBallController ball)
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
