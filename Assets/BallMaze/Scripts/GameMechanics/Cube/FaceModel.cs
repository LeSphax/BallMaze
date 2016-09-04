using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BallMaze.GameMechanics
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
    }
}
