using System;
using BallMaze.Data;
using BallMaze.GameMechanics;
using UnityEngine;

namespace BallMaze.LevelCreation.Grid
{
    public delegate void GridEventHandler(int x, int y);

    public class GridController : MonoBehaviour
    {

        public GameObject plane;
        public GameObject emptyTile;

        public Transform gridBlocks;
        public EditorBoard editorBoard;

        private IntVector2 gridSize = new IntVector2(0, 0);
        private IntVector2 boardPosition = new IntVector2(0, 0);

        private BoardData oldBoardData;
        private BoardData boardData;
        private GridTile[,] tiles;


        void Start()
        {
            LevelCreatorController controller = Camera.main.GetComponent<LevelCreatorController>();
            SetBoardData(BoardData.GetDummyBoardData());
            ComputeGridSize(oldBoardData);
            ComputeBoardPosition(gridSize, boardData);
            CreateTiles();
            SetVisible(true);

        }

        private void CreateTiles()
        {
            for (int i = 0; i < gridSize.x; i++)
            {
                for (int j = 0; j < gridSize.y; j++)
                {
                    GameObject tile = (GameObject)Instantiate(emptyTile, new Vector3((i - gridSize.x / 2.0f) * Board.BASE_TILE_SIZE_X, 0, (j - gridSize.y / 2.0f) * Board.BASE_TILE_SIZE_Y), Quaternion.identity);
                    tile.transform.SetParent(gridBlocks, false);
                    GridTile script = tile.AddComponent<GridTile>();
                    tiles[i, j] = script;
                    script.SetPosition(i, j);
                    script.GridTileClickEvent += new GridEventHandler(TileClick);
                    script.GridTileEnterEvent += new GridEventHandler(TileEnter);
                }
            }
        }

        private void ComputeGridSize(BoardData oldBoardData)
        {
            if (oldBoardData.X_SIZE % 2 == 0)
            {
                gridSize.x = 10;
            }
            else
            {
                gridSize.x = 11;
            }
            if (oldBoardData.Y_SIZE % 2 == 0)
            {
                gridSize.y = 10;
            }
            else
            {
                gridSize.y = 11;
            }
            tiles = new GridTile[gridSize.x, gridSize.y];
        }

        private void ComputeBoardPosition(IntVector2 gridSize, BoardData boardData)
        {
            boardPosition.x = gridSize.x / 2 + gridSize.x % 2 - boardData.X_SIZE / 2 - 1;
            Debug.Log(boardPosition.x);
            boardPosition.y = gridSize.y / 2 + gridSize.y % 2 - boardData.Y_SIZE / 2 - 1;
        }

        private void SetBoardPosition(BoardData boardData)
        {
            editorBoard.transform.localPosition =
                new Vector2(-Board.BASE_TILE_SIZE_X * boardData.X_SIZE / 2.0f, -Board.BASE_TILE_SIZE_Y * boardData.Y_SIZE / 2.0f);
        }

        public void SetBoardData(BoardData data)
        {
            oldBoardData = data;
            boardData = data;
            editorBoard.SetData(data);
        }

        public void SetVisible(bool visible)
        {
            foreach (GridTile tile in tiles)
            {
                tile.GetComponent<Renderer>().enabled = visible;
            }
        }

        public void TileClick(int x, int y)
        {

        }

        public void TileEnter(int x, int y)
        {
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {

                }
            }
        }
    }
}

