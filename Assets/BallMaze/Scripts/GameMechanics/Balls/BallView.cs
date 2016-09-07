using UnityEngine;

namespace BallMaze.GameMechanics
{
    public class BallView : GameObjectWithMesh
    {

        protected const float BALL_HALF_HEIGHT = 0.3f;

        public virtual void SetPosition(Vector3 position, bool cube = false)
        {
            if (cube)
            {
                transform.localPosition = position;
            }
            else
            {
                transform.localPosition = position + Vector3.up * BALL_HALF_HEIGHT;
            }
        }

    }
}