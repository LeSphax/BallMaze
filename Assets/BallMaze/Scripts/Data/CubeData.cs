
using System.Xml.Serialization;

public class CubeData : PuzzleData
{
    [XmlIgnore]
    public BallData[,,] balls;
    [XmlIgnore]
    public TileData[,,] faces;

    //For Xml Serialisation
    public CubeData()
    {

    }

    public CubeData(BallData[,,] balls, TileData[,,] faces)
    {
        this.balls = balls;
        this.faces = faces;
    }

    public TileData[][][] serializedTiles
    {
        get
        {
            return faces.ToJaggedArray();
        }
        set
        {
            faces = value.ToMatrix();
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
        foreach (TileData tile in faces)
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
}

