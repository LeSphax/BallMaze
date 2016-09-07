
using BallMaze.Data;
using UnityEngine;
using UnityEngine.Assertions;

namespace BallMaze.LevelCreation
{
    public class EditableBoardData : BoardData
    {
        private const int WIDTH_MAX = 10;
        private const int HEIGHT_MAX = 10;
        internal EditorBoard boardModel;

        private TileData[,] oldTiles = new TileData[0,0];
        private BallData[,] oldBalls = new BallData[0, 0];

        public EditableBoardData()
        {

        }

        internal EditableBoardData(EditorBoard model)
        {
            boardModel = model;

        }

        public void DestroyBoard()
        {
            SaveBoard();
            ResetBoard();
        }

        private void SaveBoard()
        {
            oldBalls = balls;
            oldTiles = tiles;
        }

        public void ResetBoard()
        {
            tiles = TileData.GetEmptyTileDataMatrix(0, 0);
            balls = BallData.GetEmptyBallDataMatrix(0, 0);
        }

        public void CreateBoard(int width, int height)
        {
            Assert.IsTrue(oldTiles.GetLength(0) == oldBalls.GetLength(0) && oldTiles.GetLength(1) == oldBalls.GetLength(1));
            tiles = new TileData[width, height];
            balls = new BallData[width, height];
            for (int i=0; i<width; i++)
            {
                for (int j=0; j<height; j++)
                {
                    if (oldBalls.GetLength(0) > i && oldBalls.GetLength(1) > j)
                    {
                        tiles[i, j] = oldTiles[i, j];
                        balls[i, j] = oldBalls[i, j];
                    }
                    else
                    {
                        tiles[i, j] = TileData.GetNormalTile();
                        balls[i, j] = BallData.GetEmptyBall();
                    }
                }
            }
            UpdateModel();
        }

        public void NextBall(int posX, int posY)
        {
            balls[posX, posY] = balls[posX, posY].GetNext();
            UpdateModel();
        }

        public void NextTileObjective(int posX, int posY)
        {
            if (!TileExists(posX, posY))
                tiles[posX, posY].ObjectiveType = tiles[posX, posY].ObjectiveType.Next();
            UpdateModel();
        }


        public void NextTileType(int posX, int posY)
        {
            if (!TileExists(posX, posY))
                tiles[posX, posY].TileType = tiles[posX, posY].TileType.Next();
            UpdateModel();
        }

        public void PreviousTileObjective(int posX, int posY)
        {
            if (!TileExists(posX, posY))
                tiles[posX, posY].ObjectiveType = tiles[posX, posY].ObjectiveType.Previous();
            UpdateModel();
        }

        internal void SetData(BoardData boardData)
        {
            tiles = boardData.tiles;
            balls = boardData.balls;
            UpdateModel();
        }

        public void PreviousTileType(int posX, int posY)
        {
            if (!TileExists(posX, posY))
                tiles[posX, posY].TileType = tiles[posX, posY].TileType.Previous();
            UpdateModel();
        }

        public bool TileExists(int posX, int posY)
        {
            if (tiles[posX, posY] == null)
            {
                return true;
            }
            return false;
        }

        internal void PreviousBall(int posX, int posY)
        {
            balls[posX, posY] = balls[posX, posY].GetPrevious();
            UpdateModel();
        }

        private void UpdateModel()
        {
            boardModel.SetData(this);
        }
    }
}