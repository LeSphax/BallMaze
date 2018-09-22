
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

[XmlInclude(typeof(EditableCubeData))]
public class CubeData : PuzzleData
{
    public BallData[,,] balls;
    public TileData[][,] faces;

    public Dictionary<ObjectiveType, int> _objectives;
    public Dictionary<ObjectiveType, int> Objectives
    {
        get
        {
            if (_objectives == null)
            {
                _objectives = new Dictionary<ObjectiveType, int>();
                foreach (BallData ball in balls)
                {
                    if (ball.ObjectiveType != ObjectiveType.NONE)
                    {
                        if (_objectives.ContainsKey(ball.ObjectiveType))
                            _objectives[ball.ObjectiveType] += 1;
                        else
                            _objectives.Add(ball.ObjectiveType, 1);
                    }
                }
            }
            return _objectives;
        }
    }

    public override bool IsValid()
    {
        return CheckObjectives();
    }

    public override string Serialize()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("Cube");
        builder.AppendLine("; Dimension");
        builder.AppendLine("\t 3,3,3");
        builder.AppendLine("; Tiles");
        for (int i = 0; i < faces.Length; i++)
        {
            for (int y = 0; y < faces[0].GetLength(0); y++)
            {
                for (int z = 0; z < faces[0].GetLength(1); z++)
                {
                    if (faces[i][y, z].ObjectiveType != ObjectiveType.NONE)
                    {
                        builder.AppendLine("\t" + (char)faces[i][y, z].TileType + ":" + (int)faces[i][y, z].ObjectiveType + " " + i + "," + y + "," + z);
                    }
                }
            }
        }
        builder.AppendLine("; Balls");
        for (int i = 0; i < balls.GetLength(0); i++)
        {
            for (int y = 0; y < balls.GetLength(1); y++)
            {
                for (int z = 0; z < balls.GetLength(2); z++)
                {
                    if (balls[i, y, z].BallType != BallType.EMPTY)
                        builder.AppendLine("\t" + (char)balls[i, y, z].BallType + ":" + (int)balls[i, y, z].ObjectiveType + " " + i + "," + y + "," + z);
                }
            }
        }
        return builder.ToString();
    }

    public static CubeData Parse(string serializedData)
    {
        string currentField = "";
        CubeData result = new CubeData();
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
                            IntVector3 dimensions = new IntVector3
                            (
                                int.Parse(split[0].Trim()),
                                int.Parse(split[1].Trim()),
                                int.Parse(split[2].Trim())
                            );
                            result.faces = TileData.CreateEmptyFaceArray(dimensions);
                            result.balls = BallData.CreateEmptyBallDataMatrix(dimensions);
                            break;
                        case "Tiles":
                            string[] split2 = trimmedLine.Split(' ');
                            string[] positions = split2[1].Split(',');
                            int x2 = int.Parse(positions[0].Trim());
                            int y2 = int.Parse(positions[1].Trim());
                            int z2 = int.Parse(positions[2].Trim());
                            string[] type = split2[0].Split(':');
                            result.faces[x2][y2,z2] = new TileData((ObjectiveType)int.Parse(type[1]), (TileType)type[0][0]);
                            break;
                        case "Balls":
                            string[] split3 = trimmedLine.Split(' ');
                            string[] positions3 = split3[1].Split(',');
                            int x3 = int.Parse(positions3[0].Trim());
                            int y3 = int.Parse(positions3[1].Trim());
                            int z3 = int.Parse(positions3[2].Trim());
                            string[] type3 = split3[0].Split(':');
                            result.balls[x3, y3, z3] = new BallData((BallType)type3[0][0], (ObjectiveType)int.Parse(type3[1]));
                            break;
                    }
                }
            }
        }
        return result;
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
        foreach (TileData[,] face in faces)
        {
            foreach (TileData tile in face)
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

}
