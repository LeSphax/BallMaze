using BallMaze.LevelCreation;
using System.Xml.Serialization;

namespace BallMaze.Data
{
    [XmlInclude(typeof(EditableBoardData))]
    public class BoardData : PuzzleData
    {
        [XmlIgnore]
        public BallData[,] balls;
        [XmlIgnore]
        public TileData[,] tiles;

        public int Width
        {
            get
            {
                return balls.GetLength(0);
            }
        }
        public int Height
        {
            get
            {
                return balls.GetLength(1);
            }
        }

        public TileData[][] serializedTiles
        {
            get
            {
                return tiles.ToJaggedArray();
            }
            set
            {
                tiles = value.ToMatrix();
            }
        }

        public BallData[][] serializedBalls
        {
            get
            {
                return balls.ToJaggedArray();
            }
            set
            {
                balls = value.ToMatrix();
            }
        }

        private void TrimTiles()
        {

        }

        public override bool IsValid()
        {
            if (balls.GetLength(0) != tiles.GetLength(0) || balls.GetLength(1) != tiles.GetLength(1))
                return false;
            else
            {
                return CheckObjectives();
            }
        }

        private bool CheckObjectives()
        {
            int numberObjectiveTypes = 2;
            int[] ObjectiveTiles = new int[numberObjectiveTypes];
            int[] ObjectiveBalls = new int[numberObjectiveTypes];

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (balls[i, j].ObjectiveType == ObjectiveType.OBJECTIVE1)
                    {
                        ObjectiveBalls[0] += 1;
                    }
                    else if (balls[i, j].ObjectiveType == ObjectiveType.OBJECTIVE2)
                    {
                        ObjectiveBalls[1] += 1;
                    }
                    if (tiles[i, j].ObjectiveType == ObjectiveType.OBJECTIVE1)
                    {
                        ObjectiveTiles[0] += 1;
                    }
                    else if (tiles[i, j].ObjectiveType == ObjectiveType.OBJECTIVE2)
                    {
                        ObjectiveTiles[1] += 1;
                    }
                }
            }

            for (int i = 0; i < numberObjectiveTypes; i++)
            {
                if (ObjectiveBalls[i] != ObjectiveTiles[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static BoardData GetDummyBoardData()
        {
            BoardData board = new BoardData();
            board.balls = BallData.GetEmptyBallDataMatrix(3, 3);
            board.tiles = TileData.GetEmptyTileDataMatrix(3, 3);
            board.balls[0, 0] = BallData.GetObjective1Ball();
            board.tiles[2, 2] = TileData.GetObjective1Tile();
            return board;
        }
    }
}