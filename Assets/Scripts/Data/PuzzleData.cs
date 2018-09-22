using System;

public abstract class PuzzleData
{
    public abstract bool IsValid();

    public virtual string Serialize()
    {
        if (typeof(CubeData).IsAssignableFrom(GetType()))
            return ((CubeData)this).Serialize();
        else if (typeof(BoardData).IsAssignableFrom(GetType()))
            return ((BoardData)this).Serialize();
        else
            throw new Exception("This type of puzzle data is not supported " + GetType());
    }

}

