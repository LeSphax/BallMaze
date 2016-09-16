using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BallMaze.Cube
{

    class FaceModel
    {
        public Axis[] axes = new Axis[3];
        public bool inverseZ;
        public int mirrorAxis;

        public int[] ReorderWithAxes(int[] table)
        {
            Assert.AreEqual(table.Length, 3);
            int[] result = new int[3];
            for (int i = 0; i < 3; i++)
            {
                result[i] = table[(int)axes[i]];
            }
            return result;
        }

        public FaceModel(Axis[] axes, bool inverseZ, int mirrorAxis = -1)
        {
            Assert.AreEqual(axes.Length, 3);
            this.axes = axes;
            this.inverseZ = inverseZ;
            this.mirrorAxis = mirrorAxis;
        }

        public Func<BallData[,,], int[], BallData> Getter
        {
            get
            {
                return (matrix, baseValues) => matrix[
                    baseValues[(int)axes[0]],
                    baseValues[(int)axes[1]],
                    baseValues[(int)axes[2]]
                    ];
            }
        }

        public static Dictionary<CubeFace, FaceModel> _faceModels;
        public const int NUMBER_FACES = 6;

        public static Dictionary<CubeFace, FaceModel> ModelsDictionary
        {
            get
            {
                if (_faceModels == null)
                {
                    _faceModels = new Dictionary<CubeFace, FaceModel>();
                    _faceModels.Add(CubeFace.X, new FaceModel(new Axis[3] { Axis.Z, Axis.Y, Axis.X }, true));
                    _faceModels.Add(CubeFace.Y, new FaceModel(new Axis[3] { Axis.X, Axis.Z, Axis.Y }, true));
                    _faceModels.Add(CubeFace.Z, new FaceModel(new Axis[3] { Axis.X, Axis.Y, Axis.Z }, true, 0));
                    _faceModels.Add(CubeFace.MX, new FaceModel(new Axis[3] { Axis.Z, Axis.Y, Axis.X }, false, 0));
                    _faceModels.Add(CubeFace.MY, new FaceModel(new Axis[3] { Axis.X, Axis.Z, Axis.Y }, false, 1));
                    _faceModels.Add(CubeFace.MZ, new FaceModel(new Axis[3] { Axis.X, Axis.Y, Axis.Z }, false));
                }
                return _faceModels;
            }
        }

        public static int GetRotation(CubeFace face, Vector3 rotation)
        {
            switch (face)
            {
                case CubeFace.X:
                case CubeFace.MX:
                case CubeFace.Z:
                case CubeFace.MZ:
                    return Mathf.RoundToInt(rotation.z);
                case CubeFace.Y:
                    return Mathf.RoundToInt(360 - rotation.y);
                case CubeFace.MY:
                    return Mathf.RoundToInt(rotation.y);
                default:
                    throw new UnhandledSwitchCaseException(face);
            }
        }

        public IntVector3 GetRealSizes(int x, int y, int z)
        {
            return GetRealSizes(new IntVector3(x, y, z));
        }

        public IntVector3 GetRealSizes(IntVector3 sizes)
        {
            IntVector3 result = new IntVector3();
            result[axes[0]] = sizes.x;
            result[axes[1]] = sizes.y;
            result[axes[2]] = sizes.z;
            return result;
        }

        public IntVector3 GetRealCoords(IntVector3 coords, IntVector3 sizes)
        {
            IntVector3 result = new IntVector3();
            if (inverseZ)
            {
                coords.z = sizes[axes[2]] - coords.z - 1;
            }
            if (mirrorAxis != -1)
            {
                coords[mirrorAxis] = sizes[axes[mirrorAxis]] - coords[mirrorAxis] - 1;
            }
            result[axes[0]] = coords.x;
            result[axes[1]] = coords.y;
            result[axes[2]] = coords.z;
            return result;
        }
    }
}
