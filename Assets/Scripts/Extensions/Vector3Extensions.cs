
using UnityEngine;

public static class Vector3Extensions
{
    public static float GetRotationToFace(this Vector3 vector, CubeFace face)
    {
        switch (face)
        {
            case CubeFace.X:
                return vector.x;
            case CubeFace.Y:
                return vector.y;
            case CubeFace.Z:
                return vector.z;
            case CubeFace.MX:
                return vector.x + 90;
            case CubeFace.MY:
                return vector.y + 90;
            case CubeFace.MZ:
                return vector.z + 90;
            default:
                throw new UnhandledSwitchCaseException(face);
        }
    }
}

