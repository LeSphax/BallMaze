using System;
using System.Collections.Generic;
using UnityEngine.Assertions;

namespace BallMaze.GameMechanics
{
    class FaceModel
    {
        public Axis[] axes = new Axis[3];

        public FaceModel(Axis[] axes)
        {
            Assert.AreEqual(axes.Length, 3);
            this.axes = axes;
        }

        public Func<BallData[,,],int[],BallData> Getter
        {
            get
            {
                return (matrix, baseAxis) => matrix[baseAxis[(int)axes[0]], baseAxis[(int)axes[1]], baseAxis[(int)axes[2]]];
            }
        }

        public static Dictionary<CubeFace, FaceModel> _faceModels;
        public static Dictionary<CubeFace, FaceModel> FaceModels
        {
            get
            {
                if (_faceModels == null)
                {
                    _faceModels = new Dictionary<CubeFace, FaceModel>();
                    _faceModels.Add(CubeFace.X, new FaceModel(new Axis[3] { Axis.Z, Axis.Y, Axis.X }));
                    _faceModels.Add(CubeFace.Y, new FaceModel(new Axis[3] { Axis.X, Axis.Z, Axis.Y }));
                    _faceModels.Add(CubeFace.Z, new FaceModel(new Axis[3] { Axis.X, Axis.Y, Axis.Z }));
                    _faceModels.Add(CubeFace.MX, new FaceModel(new Axis[3] { Axis.Z, Axis.Y, Axis.X }));
                    _faceModels.Add(CubeFace.MY, new FaceModel(new Axis[3] { Axis.X, Axis.Z, Axis.Y }));
                    _faceModels.Add(CubeFace.MZ, new FaceModel(new Axis[3] { Axis.X, Axis.Y, Axis.Z }));
                }
                return _faceModels;
            }
        }
    }
}
