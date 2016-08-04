using System;

namespace BallMaze.Inputs
{
    [Serializable]
    public enum Direction
    {
        UP, DOWN, RIGHT, LEFT,
        NONE
    }

    [Serializable]
    public enum CubeFace
    {
        X, Y, Z,
        MX, MY, MZ
    }
    
}