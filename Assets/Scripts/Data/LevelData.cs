using System;
using System.Linq;
using System.Text;
using UnityEngine;

public class LevelData
{

    private string fileName;
    public string FileName
    {
        get
        {
            return fileName;
        }
        set
        {
            fileName = value.Trim();
        }
    }
    public virtual string Name
    {
        get
        {
            string[] split = fileName.Split(Paths.FOLDER_SEPARATOR_CHAR);
            return split[split.Length - 1];
        }
    }

    public PuzzleData PuzzleData;

    public int numberMoves;

    public ObjectiveType firstObjective;

    public LevelData()
    {

    }

    public LevelData(PuzzleData data, string name)
    {
        InitData(data, name);
    }

    private void InitData(PuzzleData data, string name)
    {
        this.PuzzleData = data;
        this.fileName = name;
    }

    public void SetFirstObjective(ObjectiveType first)
    {
        firstObjective = first;
    }

    public void SetNumberMoves(int numberMoves)
    {
        this.numberMoves = numberMoves;
    }

    public virtual string Serialize()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("; numberMoves");
        builder.AppendLine("\t" + numberMoves);
        builder.AppendLine("; firstObjective");
        builder.AppendLine("\t" + (int)firstObjective);

        string serializedData = PuzzleData.Serialize();
        //Add additionnal tab on each line
        serializedData.Split(Environment.NewLine.ToCharArray()).Select(x => "\t" + x).Aggregate((i, j) => i + Environment.NewLine + j);
        builder.Append(serializedData);

        return builder.ToString();
    }

    public static LevelData Load(string fileName)
    {
        string serializedData = Resources.Load<TextAsset>(Paths.LEVEL_FILES + fileName).text;
        return Parse(fileName, serializedData);
    }

    public static LevelData Parse(string fileName, string serializedData)
    {
        string currentField = "";
        int currentPosition = 0;
        LevelData result = new LevelData();
        result.FileName = fileName;
        foreach (string line in serializedData.Split(Environment.NewLine.ToCharArray()))
        {
            currentPosition += line.Length;
            string trimmedLine = line.Trim();
            if (trimmedLine.Length > 0)
            {
                if (trimmedLine[0] == ';')
                {
                    currentField = trimmedLine.Substring(1);
                }
                else if (trimmedLine == "Board")
                {
                    result.PuzzleData = BoardData.Parse(serializedData.Substring(currentPosition));
                }
                else if(trimmedLine == "Cube")
                {
                    result.PuzzleData = CubeData.Parse(serializedData.Substring(currentPosition));
                }
                else
                {
                    switch (currentField)
                    {
                        case "numberMoves":
                            result.numberMoves = int.Parse(trimmedLine);
                            break;
                        case "firstObjective":
                            result.firstObjective = (ObjectiveType)int.Parse(trimmedLine);
                            break;
                    }
                }
            }
        }
        return result;
    }
}


