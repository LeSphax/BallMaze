using UnityEngine;

namespace BallMaze.GameMechanics
{
    public class BallView : GameObjectWithMesh
    {

        protected const float BALL_HALF_HEIGHT = 0.3f;

        public void SetPosition(Vector3 position)
        {
            transform.localPosition = position + Vector3.up * BALL_HALF_HEIGHT;
        }
    }
}