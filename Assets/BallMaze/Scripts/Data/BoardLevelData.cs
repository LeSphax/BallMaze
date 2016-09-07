using BallMaze.Data;
using UnityEngine;
using Utilities;

public class BoardLevelData : LevelData
{
    public const char FILE_EXTENSION = 'B';
    public BoardData data;

    //For Xml Serialisation
    public BoardLevelData() { }

    public BoardLevelData(BoardData data, string previousLevelName, string name, string nextLevelName) : base(data,previousLevelName,name,nextLevelName)
    {
    }

    protected override PuzzleData puzzleData
    {
        get
        {
            return data;
        }

        set
        {
            if (typeof(BoardData).IsAssignableFrom(value.GetType()))
                data = (BoardData)value;
            else
                Debug.LogError("This value isn't a boardData:" + value.GetType());
        }
    }

    protected override void Serialize(string path)
    {
        path += FILE_EXTENSION;
        Saving.Save(path, this);
    }
}

