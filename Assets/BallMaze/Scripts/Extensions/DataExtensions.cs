using BallMaze.GameMechanics.Tiles;
using System.Collections.Generic;

namespace BallMaze
{
    public static class DataExtensions
    {

        public static bool TryFillTile(this Dictionary<ObjectiveType, bool> hashMap, TileController tile)
        {
            bool value;
            if (hashMap.TryGetValue(tile.GetObjectiveType(), out value))
            {
                if (value)
                {
                    tile.Fill();
                    return true;
                }
            }
            return false;
        }

        public static bool AllFilled(this Dictionary<ObjectiveType, bool> hashMap)
        {
            foreach (bool value in hashMap.Values)
            {
                if (!value)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
