using UnityEngine;
using UnityEngine.EventSystems;

public delegate void TileClicked(CubeFace face, int x, int y);

public class EditorTile : MonoBehaviour
{
    public CubeFace face;
    public int PosX;
    public int PosY;

    public event TileClicked GridTileClickEvent;

    void OnMouseOver()
    {
        if (!EventSystem.current.IsPointerOverGameObject(-1))
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                if (GridTileClickEvent != null)
                    GridTileClickEvent.Invoke(face, PosX, PosY);
                else
                    Debug.LogError("GridTileClickEvent should be listened to");
            }
        }
    }

    void OnMouseEnter()
    {
        //if (!EventSystem.current.IsPointerOverGameObject(-1))
        //{
        //    if (GridTileEnterEvent != null)
        //        GridTileEnterEvent.Invoke(face,PosX, PosY);
        //    else
        //        Debug.LogError("GridTileEnterEvent should be listened to");
        //}
    }


    public void Init(CubeFace face, int x, int y)
    {
        this.face = face;
        PosX = x;
        PosY = y;
    }
}
