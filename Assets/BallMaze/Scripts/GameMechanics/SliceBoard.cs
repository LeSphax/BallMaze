using System.Collections.Generic;

namespace BallMaze.GameMechanics
{
    public class SliceBoard : PlayBoard
    {
        public CubeFace face;
        public int rotation;

        protected override bool CheckIfWon()
        {
            return false;
        }

        public Dictionary<BallData, Coords> GetBallsPositions()
        {
            Dictionary<BallData, Coords> result = new Dictionary<BallData, Coords>();
            for (int x = 0; x < boardData.Width; x++)
            {
                for (int y = 0; y < boardData.Width; y++)
                {
                    Coords point = new Coords(x, y);
                    result.Add(boardData.balls[x,y], point);
                }
            }
            return result;
        }
    }
}