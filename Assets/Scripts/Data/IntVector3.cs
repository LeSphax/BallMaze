public struct IntVector3
{
    private int[] coords;

    public int X
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

    public int Y
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

    public int Z
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

    public IntVector3(int p1 = 0, int p2 = 0, int p3 = 0)
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
        set
        {
            if (coords == null)
            {
                coords = new int[3];
            }
            coords[(int)i] = value;
        }
    }

    public int this[int i]
    {
        get
        {
            return coords[i];
        }
        set
        {
            if (coords == null)
            {
                coords = new int[3];
            }
            coords[i] = value;
        }
    }

    public override string ToString()
    {
        return coords.Print();
    }

    public static IntVector3 Zero
    {
        get
        {
            return new IntVector3(0, 0, 0);
        }
    }

    public static IntVector3 One
    {
        get
        {
            return new IntVector3(1,1,1);
        }
    }

    public static IntVector3 operator *(IntVector3 vector, int y)
    {
        return new IntVector3(vector.X * y, vector.Y * y, vector.Z * y);
    }
}
