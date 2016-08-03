using UnityEngine;

namespace BallMaze.LevelCreation.Grid
{
    class GridTile : MonoBehaviour
    {

        public int PosX;
        public int PosY;

        public event GridEventHandler GridTileClickEvent;
        public event GridEventHandler GridTileEnterEvent;

        void OnMouseOver()
        {
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1))
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
                {
                    if (GridTileClickEvent != null)
                        GridTileClickEvent.Invoke(PosX, PosY);
                    else
                        Debug.LogError("This should be listened to");
                }
            }
        }

        void OnMouseEnter()
        {
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1))
            {
                if (GridTileEnterEvent != null)
                    GridTileEnterEvent.Invoke(PosX, PosY);
                else
                    Debug.LogError("This should be listened to");
            }
        }


        public void SetPosition(int x, int y)
        {
            PosX = x;
            PosY = y;
        }
    }
}