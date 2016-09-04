
using BallMaze.Data;
using System.Xml.Serialization;

[XmlInclude(typeof(BoardData))]
[XmlInclude(typeof(CubeData))]
public abstract class PuzzleData
{
    public abstract bool IsValid();
}

