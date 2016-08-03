
using BallMaze.Data;

namespace BallMaze.LevelCreation
{
    public class EditableBoardData : BoardData
    {
        private const int WIDTH_MAX = 10;
        private const int HEIGHT_MAX = 10;
        internal EditorBoardModel boardModel;

        public EditableBoardData()
        {

        }

        internal EditableBoardData(EditorBoardModel model)
        {
            boardModel = model;

        }

        public void CreateBoard(int width, int height)
        {
            tiles = TileData.GetEmptyTileDataMatrix(width, height);
            balls = BallData.GetEmptyBallDataMatrix(width, height);
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