using BallMaze.Inputs;
using System;


public static class EnumExtensions
{
    public static Enum Next<Enum>(this Enum src) where Enum : struct
    {
        if (!typeof(Enum).IsEnum()) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(Enum).FullName));

        Enum[] Arr = (Enum[])System.Enum.GetValues(src.GetType());
        int j = Array.IndexOf<Enum>(Arr, src) + 1;
        return (Arr.Length == j) ? Arr[0] : Arr[j];
    }

    public static Enum Previous<Enum>(this Enum src) where Enum : struct
    {
        if (!typeof(Enum).IsEnum()) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(Enum).FullName));

        Enum[] Arr = (Enum[])System.Enum.GetValues(src.GetType());
        int j = Array.IndexOf<Enum>(Arr, src) - 1;
        return (0 > j) ? Arr[Arr.Length - 1] : Arr[j];
    }

    public static Direction Opposite(this Direction direction)
    {
        switch (direction)
        {
            case Direction.DOWN:
                return Direction.UP;
            case Direction.UP:
                return Direction.DOWN;
            case Direction.LEFT:
                return Direction.RIGHT;
            case Direction.RIGHT:
                return Direction.LEFT;
            case Direction.NONE:
                return Direction.NONE;
            default:
                throw new UnhandledSwitchCaseException(direction);
        }
    }
}


