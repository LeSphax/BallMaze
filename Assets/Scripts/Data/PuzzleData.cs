
using BallMaze.Data;
using System;
using System.Xml.Serialization;

public enum PuzzleType
{
    BOARD,
    CUBE
}

[XmlInclude(typeof(BoardData))]
[XmlInclude(typeof(CubeData))]
public abstract class PuzzleData
{


    public abstract bool IsValid();

    public static Type GetFileType(PuzzleType type)
    {
        switch (type)
        {
            case PuzzleType.BOARD:
                return typeof(BoardLevelData);
            case PuzzleType.CUBE:
                return typeof(CubeLevelData);
            default:
                throw new UnhandledSwitchCaseException(type);
        }
    }

    public static string GetDirectoryName(PuzzleType type)
    {
        switch (type)
        {
            case PuzzleType.BOARD:
                return BoardLevelData.DirectoryName();
            case PuzzleType.CUBE:
                return CubeLevelData.DirectoryName();
            default:
                throw new UnhandledSwitchCaseException(type);
        }
    }
}

