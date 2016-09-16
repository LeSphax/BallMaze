using BallMaze.GameMechanics;
using BallMaze.GameMechanics.Tiles;
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

        public int X_SIZE
        {
            get
            {
                return balls.GetLength(0);
            }
        }
        public int Y_SIZE
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

            for (int i = 0; i < X_SIZE; i++)
            {
                for (int j = 0; j < Y_SIZE; j++)
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

        public static BoardData GetBoardData(BoardPosition[,] board)
        {
            BoardData data = new BoardData();
            int X_SIZE = board.GetLength(0);
            int Y_SIZE = board.GetLength(1);

            data.balls = new BallData[X_SIZE, Y_SIZE];
            data.tiles = new TileData[X_SIZE, Y_SIZE];

            for (int x = 0; x < X_SIZE; x++)
            {
                for (int y = 0; y < Y_SIZE; y++)
                {
                    IBallController ball = board[x, y].ball;
                    TileController tile = board[x, y].tile;
                    data.balls[x, y] = new BallData(ball.GetBallType(), ball.GetObjectiveType());
                    data.tiles[x, y] = new TileData(tile.GetObjectiveType(), tile.tileType);
                }
            }
            return data;
        }
    }
}