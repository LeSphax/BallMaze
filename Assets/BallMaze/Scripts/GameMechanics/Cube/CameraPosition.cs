using System;
using UnityEngine;

public class CameraPosition
{
    public CubeFace face;
    public CubeFace oppositeFace;
    public CubeFace rightFace;
    public CubeFace leftFace;
    public CubeFace topFace;
    public CubeFace downFace;

    public void InitNormal()
    {
        face = CubeFace.Z;
        oppositeFace = CubeFace.MZ;
        rightFace = CubeFace.X;
        leftFace = CubeFace.MX;
        topFace = CubeFace.Y;
        downFace = CubeFace.MY;
    }

    public void GoRight()
    {
        CubeFace oldFace = face;
        CubeFace oldOpposite = oppositeFace;
        face = rightFace;
        oppositeFace = leftFace;
        rightFace = oldOpposite;
        leftFace = oldFace;
    }

    public void GoLeft()
    {
        CubeFace oldFace = face;
        CubeFace oldOpposite = oppositeFace;
        face = leftFace;
        oppositeFace = rightFace;
        rightFace = oldFace;
        leftFace = oldOpposite;
    }

    public void GoUp()
    {
        CubeFace oldFace = face;
        CubeFace oldOpposite = oppositeFace;
        face = topFace;
        oppositeFace = downFace;
        topFace = oldOpposite;
        downFace = oldFace;
    }

    public void GoDown()
    {
        CubeFace oldFace = face;
        CubeFace oldOpposite = oppositeFace;
        face = downFace;
        oppositeFace = topFace;
        topFace = oldFace;
        downFace = oldOpposite;
    }

    public Vector3 ToAngles()
    {
        float planeAngle;
        float heightAngle;
        float headAngle;
        switch (face)
        {
            case CubeFace.X:
            case CubeFace.MX:
            case CubeFace.Z:
            case CubeFace.MZ:
                heightAngle = Mathf.PI / 2;
                break;
            case CubeFace.Y:
                heightAngle = 0;// Mathf.PI;
                break;
            case CubeFace.MY:
                heightAngle = Mathf.PI;
                break;
            default:
                throw new UnhandledSwitchCaseException(face);
        }
        planeAngle = SetPlaneAngle(face, downFace);
        headAngle = SetHeadAngle();
        return new Vector3(planeAngle, heightAngle, headAngle);
    }

    private float SetHeadAngle()
    {
        float headAngle;
        switch (face)
        {
            case CubeFace.Y:
            case CubeFace.MY:
                headAngle = 0;
                break;
            case CubeFace.X:
            case CubeFace.MX:
            case CubeFace.Z:
            case CubeFace.MZ:
                switch (rightFace)
                {
                    case CubeFace.Y:
                        headAngle = Mathf.PI / 2;
                        break;
                    case CubeFace.MY:
                        headAngle = -Mathf.PI / 2;
                        break;
                    case CubeFace.X:
                    case CubeFace.MX:
                    case CubeFace.Z:
                    case CubeFace.MZ:
                        switch (topFace)
                        {
                            case CubeFace.Y:
                                headAngle = 0;
                                break;
                            case CubeFace.MY:
                                headAngle = Mathf.PI;
                                break;
                            default:
                                throw new Exception("This face combination isn't possible face : " + face + " right : " + rightFace + " top : " + topFace);
                        }
                        break;
                    default:
                        throw new UnhandledSwitchCaseException(face);
                }
                break;
            default:
                throw new UnhandledSwitchCaseException(face);
        }

        return headAngle;
    }

    private float SetPlaneAngle(CubeFace face, CubeFace downFace = CubeFace.NONE)
    {
        switch (face)
        {
            case CubeFace.X:
                return 0;
            case CubeFace.MX:
                return Mathf.PI;
            case CubeFace.Z:
                return Mathf.PI / 2;
            case CubeFace.MZ:
                return -Mathf.PI / 2;
            case CubeFace.Y:
                if (downFace != CubeFace.NONE)
                    return SetPlaneAngle(downFace);
                else
                    throw new Exception("This face combination isn't possible " + ToString());
            case CubeFace.MY:
                if (downFace != CubeFace.NONE)
                    return SetPlaneAngle(topFace);
                else
                    throw new Exception("This face combination isn't possible "+ToString());
            default:
                throw new UnhandledSwitchCaseException(face);
        }
    }

    public override string ToString()
    {
        string result = "";
        result += "Face : " + face;
        result += " Opposite : " + oppositeFace;
        result += " Top : " + topFace;
        result += " Down : " + downFace;
        result += " Right : " + rightFace;
        result += " Left : " + leftFace;
        return result;
    }
}

