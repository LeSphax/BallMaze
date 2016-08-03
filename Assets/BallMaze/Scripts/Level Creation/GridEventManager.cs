using UnityEngine;

namespace BallMaze.LevelCreation.Grid
{
    public class GridEventManager : MonoBehaviour
    {
        private float TILE_X_SIZE;

        //public event GridEventHandler GridLeftClickEvent;
        //public event GridEventHandler GridRightClickEvent;

        void Update()
        {
            RaycastHit hit;
            if (Input.GetMouseButtonDown(0) && Functions.RaycastMouse(out hit))
            {
                //Vector3 mouseGridPosition = transform.InverseTransformPoint(hit.point);
                //   int x = (int)(mouseGridPosition.x / TILE_X_SIZE);
                // GridLeftClickEvent.Invoke();
            }
        }
    }
}
