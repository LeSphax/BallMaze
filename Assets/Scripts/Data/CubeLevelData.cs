using BallMaze.Data;
using UnityEngine;
using Utilities;

public class CubeLevelData : LevelData
{
    public const char FILE_EXTENSION = 'C';
    public CubeData data;

    public override string Name
    {
        get
        {
            return base.Name.Substring(2);
        }

        set
        {
            base.Name = value;
        }
    }

    //For Xml Serialisation
    public CubeLevelData() { }

    public CubeLevelData(CubeData data, string previousLevelName, string name, string nextLevelName) : base(data,previousLevelName,name,nextLevelName)
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
            if (value.GetType().SubclassOf(typeof(CubeData)))
                data = (CubeData)value;
            else
                Debug.LogError("This value isn't a cubeData:" + value.GetType());
        }
    }

    protected override void Serialize(string path)
    {
        Saving.Save(path, this);
    }
}

