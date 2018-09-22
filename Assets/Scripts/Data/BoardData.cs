

using System;
using System.Text;

public class BoardData : PuzzleData
{
    public BallData[,] balls;
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
        BoardData board = new BoardData
        {
            balls = BallData.CreateEmptyBallDataMatrix(3, 3),
            tiles = TileData.CreateEmptyTileDataMatrix(3, 3)
        };
        board.balls[0, 0] = BallData.GetObjective1Ball();
        board.tiles[2, 2] = TileData.GetObjective1Tile();
        return board;
    }

    public override string Serialize()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("Board");
        builder.AppendLine("; Dimension");
        builder.AppendLine("\t " + tiles.GetLength(0) + "," + tiles.GetLength(1));
        builder.AppendLine("; Tiles");
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int y = 0; y < tiles.GetLength(1); y++)
            {
                if (tiles[i, y].ObjectiveType != ObjectiveType.NONE)
                {
                    builder.AppendLine("\t" + (char)tiles[i, y].TileType + ":" + (int)tiles[i, y].ObjectiveType + " " + i + "," + y);
                }
            }
        }
        builder.AppendLine("; Balls");
        for (int i = 0; i < balls.GetLength(0); i++)
        {
            for (int y = 0; y < balls.GetLength(1); y++)
            {
                if (balls[i, y].BallType != BallType.EMPTY)
                    builder.AppendLine("\t" + (char)balls[i, y].BallType + ":" + (int)balls[i, y].ObjectiveType + " " + i + "," + y);
            }
        }
        return builder.ToString();
    }

    public static BoardData Parse(string serializedData)
    {
        string currentField = "";
        BoardData result = new BoardData();
        foreach (string line in serializedData.Split(Environment.NewLine.ToCharArray()))
        {
            string trimmedLine = line.Trim();

            if (trimmedLine.Length > 0)
            {
                if (trimmedLine[0] == ';')
                {
                    currentField = trimmedLine.Substring(1).Trim();
                }
                else
                {
                    switch (currentField)
                    {
                        case "Dimension":
                            string[] split = trimmedLine.Split(',');
                            int x = int.Parse(split[0].Trim());
                            int y = int.Parse(split[1].Trim());
                            result.tiles = TileData.CreateEmptyTileDataMatrix(x, y);
                            result.balls = BallData.CreateEmptyBallDataMatrix(x, y);
                            break;
                        case "Tiles":
                            string[] split2 = trimmedLine.Split(' ');
                            string[] positions = split2[1].Split(',');
                            int x2 = int.Parse(positions[0].Trim());
                            int y2 = int.Parse(positions[1].Trim());
                            string[] type = split2[0].Split(':');

                            result.tiles[x2, y2] = new TileData((ObjectiveType)int.Parse(type[1]), (TileType)type[0][0]);
                            break;
                        case "Balls":
                            string[] split3 = trimmedLine.Split(' ');
                            string[] positions3 = split3[1].Split(',');
                            int x3 = int.Parse(positions3[0].Trim());
                            int y3 = int.Parse(positions3[1].Trim());
                            string[] type3 = split3[0].Split(':');
                            result.balls[x3, y3] = new BallData((BallType)type3[0][0], (ObjectiveType)int.Parse(type3[1]));
                            break;
                    }
                }
            }
        }
        return result;
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
