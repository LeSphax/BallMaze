using Utilities;

public struct Coords
{
    private int[] coords;

    public int x
    {
        get
        {
            return coords[0];
        }
        set
        {
            coords[0] = value;
        }
    }

    public int y
    {
        get
        {
            return coords[1];
        }
        set
        {
            coords[1] = value;
        }
    }

    public int z
    {
        get
        {
            return coords[2];
        }
        set
        {
            coords[2] = value;
        }
    }

    public Coords(int p1=0, int p2=0, int p3=0)
    {
        coords = new int[3];
        this[Axis.X] = p1;
        this[Axis.Y] = p2;
        this[Axis.Z] = p3;
    }

    public int this[Axis i]
    {
        get
        {
            return coords[(int)i];
        }
        set {
            if (coords == null)
            {
                coords = new int[3];
            }
            coords[(int)i] = value;
        }
    }

    public override string ToString()
    {
        return coords.Print();
    }
}
