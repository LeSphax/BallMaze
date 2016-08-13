public enum CubeFace: int
{
    X = 0,
    Y = 1,
    Z = 2,
    MX = 3,
    MY = 4,
    MZ = 5,
    NONE = -1
    ////The face on the X axis that is on the left/ side where X is lower (left if you look with z going away)
    //public const int X = 0;
    //public const int Y = 1;
    //public const int Z = 2;

    ////The face on the X axis that is on the side where X is higher (right if you look with z going away)
    //public const int MX = 3;
    //public const int MY = 4;
    //public const int MZ = 5;

}
public static class CubeFaceExtensions
{

    public static bool IsYReversed(this CubeFace face)
    {
        switch (face)
        {
            case CubeFace.X:
            case CubeFace.Y:
            case CubeFace.Z:
                return true;
            case CubeFace.MX:
            case CubeFace.MY:
            case CubeFace.MZ:
                return false;
            case CubeFace.NONE:
                return false;
            default:
                throw new UnhandledSwitchCaseException(face);
        }
    }

    public static bool IsXReversed(this CubeFace face)
    {
        switch (face)
        {
            case CubeFace.X:
            case CubeFace.Y:
            case CubeFace.Z:
            case CubeFace.MX:
            case CubeFace.MZ:
                return false;
            case CubeFace.MY:
                return true;
            case CubeFace.NONE:
                return false;
            default:
                throw new UnhandledSwitchCaseException(face);
        }
    }
}
