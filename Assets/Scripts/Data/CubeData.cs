
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

[XmlInclude(typeof(EditableCubeData))]
public class CubeData : PuzzleData
{
    [XmlIgnore]
    public BallData[,,] balls;
    [XmlIgnore]
    public TileData[][,] faces;

    [XmlIgnore]
    public Dictionary<ObjectiveType, int> _objectives;
    [XmlIgnore]
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

    //For Xml Serialisation
    public CubeData()
    {

    }

    public CubeData(BallData[,,] balls, TileData[][,] faces)
    {
        this.balls = balls;
        this.faces = faces;
    }

    public TileData[][][] serializedTiles
    {
        get
        {
            TileData[][][] result = new TileData[6][][];
            foreach (var item in faces.Select((value, i) => new { i, value }))
            {
                result[item.i] = item.value.ToJaggedArray();
            }
            return result;
        }
        set
        {
            TileData[][,] result = new TileData[6][,];
            foreach (var item in value.Select((val, i) => new { i, val }))
            {
                result[item.i] = item.val.ToMatrix();
            }
            faces = result;
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

    public override bool IsValid()
    {
        return CheckObjectives();
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
