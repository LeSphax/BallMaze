using BallMaze.GameMechanics;
using UnityEngine;

namespace BallMaze.LevelCreation.Grid
{
    public delegate void GridEventHandler(int x, int y);

    public class GridController : MonoBehaviour
    {

        public GameObject plane;
        public GameObject emptyTile;

        public int gridSizeX;
        public int gridSizeY;

        private GridTile[,] tiles;

        void Start()
        {
            LevelCreatorController controller = Camera.main.GetComponent<LevelCreatorController>();
            tiles = new GridTile[gridSizeX, gridSizeY];
            for (int i = 0; i < gridSizeX; i++)
            {
                for (int j = 0; j < gridSizeY; j++)
                {
                    GameObject tile = (GameObject)Instantiate(emptyTile, new Vector3(i * BoardModel.SIZE_TILE_X, 0, j * BoardModel.SIZE_TILE_Y), Quaternion.identity);
                    tile.transform.SetParent(transform.Find("GridBlocks"), false);
                    GridTile script = tile.AddComponent<GridTile>();
                    tiles[i, j] = script;
                    script.SetPosition(i, j);
                    script.GridTileClickEvent += new GridEventHandler(controller.OnGridClick);
                    script.GridTileEnterEvent += new GridEventHandler(controller.OnGridEnter);
                }
            }
        }

        public void SetVisible(bool visible)
        {
            foreach (GridTile tile in tiles)
            {
                tile.GetComponent<Renderer>().enabled = visible;
            }
        }
    }
}

