using CustomAnimations.BallMazeAnimations;
using UnityEngine;

namespace BallMaze.GameMechanics.Tiles
{
    public class TileView : GameObjectWithMesh
    {
        internal ColorAnimation GetFillingAnimation(float duration)
        {
            return ColorAnimation.CreateColorAnimation(Mesh, Color.yellow, duration, 1);
        }
    }
}
