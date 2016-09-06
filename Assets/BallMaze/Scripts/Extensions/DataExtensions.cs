using BallMaze.GameMechanics.Tiles;
using System.Collections.Generic;

namespace BallMaze
{
    public static class DataExtensions
    {

        public static bool TryFillTile(this List<ObjectiveType> list, TileController tile)
        {
            if (list.Contains(tile.GetObjectiveType()))
            {
                tile.Fill();
                return true;
            }
            else
                return false;
        }
    }
}
