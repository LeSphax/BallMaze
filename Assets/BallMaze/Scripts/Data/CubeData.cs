using BallMaze.GameMechanics.LevelCreation;
using BallMaze.LevelCreation;
using System.Xml.Serialization;

namespace BallMaze.Data
{
    [XmlInclude(typeof(EditableBoardData))]
    public class CubeData
    {
        [XmlIgnore]
        public BallData[,,] balls;
        [XmlIgnore]
        public TileData[,,] tiles;

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
        public int Z_SIZE
        {
            get
            {
                return balls.GetLength(2);
            }
        }

        public TileData[][][] serializedTiles
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

        public BallData[][][] serializedBalls
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

        public bool IsValid()
        {
            if (balls.GetLength(0) != tiles.GetLength(0) || balls.GetLength(1) != tiles.GetLength(1) || balls.GetLength(2) != tiles.GetLength(2))
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

            foreach (BallData ball in balls)
            {
                if (ball.ObjectiveType == ObjectiveType.OBJECTIVE1)
                {
                    ObjectiveBalls[0] += 1;
                }
                else if (ball.ObjectiveType == ObjectiveType.OBJECTIVE2)
                {
                    ObjectiveBalls[1] += 1;
                }
            }
            foreach (TileData tile in tiles)
            {

                if (tile.ObjectiveType == ObjectiveType.OBJECTIVE1)
                {
                    ObjectiveTiles[0] += 1;
                }
                else if (tile.ObjectiveType == ObjectiveType.OBJECTIVE2)
                {
                    ObjectiveTiles[1] += 1;
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

        public static CubeData GetDummyCubeData()
        {
            CubeData board = new CubeData();
            board.balls = BallData.GetEmptyBallDataMatrix(3, 3, 3);
            board.tiles = TileData.GetEmptyTileDataMatrix(6, 3, 3);
            board.balls[0, 0, 0] = BallData.GetObjective1Ball();
            board.tiles[2, 2, 2] = TileData.GetObjective1Tile();
            return board;
        }
    }
}