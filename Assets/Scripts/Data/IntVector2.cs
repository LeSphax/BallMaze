public struct IntVector2
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

    public IntVector2(int p1 = 0, int p2 = 0)
    {
        coords = new int[2];
        this[Axis.X] = p1;
        this[Axis.Y] = p2;
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
                coords = new int[2];
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
                coords = new int[2];
            }
            coords[i] = value;
        }
    }

    public override string ToString()
    {
        return coords.Print();
    }

    public static IntVector3 zero
    {
        get
        {
            return new IntVector3(0, 0, 0);
        }
    }

    public static IntVector3 one
    {
        get
        {
            return new IntVector3(1,1,1);
        }
    }

    public static IntVector2 operator *(IntVector2 vector, int y)
    {
        return new IntVector2(vector.x * y, vector.y * y);
    }
}
