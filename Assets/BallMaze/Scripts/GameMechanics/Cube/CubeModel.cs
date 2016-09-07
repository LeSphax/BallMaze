using BallMaze.Data;
using BallMaze.GameMechanics;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BallMaze.Cube
{
    public delegate void CubeChanged(CubeModel model);

    public class CubeModel : MonoBehaviour
    {
        public BallData[,,] balls;
        public TileData[,,] faces;

        public Dictionary<ObjectiveType, bool> objectivesFilled;

        public event CubeChanged HasChanged;
        public event EmptyEventHandler LevelCompleted;

        public int[] sizes
        {
            get
            {
                return new int[3] { X_SIZE, Y_SIZE, Z_SIZE };
            }
        }

        public int X_SIZE
        {
            get
            {
                return balls.GetLength(0);
            }
        }
        public int Y_SIZE
        {
            get
            {
                return balls.GetLength(1);
            }
        }
        public int Z_SIZE
        {
            get
            {
                return balls.GetLength(2);
            }
        }

        public void SetDummyCubeModel()
        {
            balls = BallData.GetEmptyBallDataMatrix(3, 3, 3);
            faces = TileData.GetEmptyTileDataMatrix(6, 3, 3);
            balls[0, 0, 0] = BallData.GetObjective1Ball();
            balls[0, 1, 1] = BallData.GetWall();
            balls[1, 1, 1] = BallData.GetWall();
            balls[2, 1, 1] = BallData.GetWall();
            //balls[0, 0, 2] = BallData.GetWall();
            //balls[2, 1, 2] = BallData.GetWall();
            //
            faces[5, 0, 2] = TileData.GetObjective1Tile();
            //faces[0, 2, 2] = TileData.GetObjective2Tile();
            //faces[1, 1, 2] = TileData.GetObjective1Tile();
            //faces[1, 2, 1] = TileData.GetObjective2Tile();
            //faces[2, 1, 0] = TileData.GetObjective1Tile();
            //faces[2, 2, 0] = TileData.GetObjective2Tile();
            //faces[3, 0, 2] = TileData.GetObjective1Tile();
            //faces[3, 2, 1] = TileData.GetObjective2Tile();
            //faces[4, 2, 0] = TileData.GetObjective2Tile();
            //faces[5, 0, 1] = TileData.GetObjective1Tile();
            //faces[5, 0, 2] = TileData.GetObjective2Tile();
            HasChanged.Invoke(this);
        }

        internal void SetData(CubeData data)
        {
            objectivesFilled = new Dictionary<ObjectiveType, bool>();
            foreach (var key in data.Objectives.Keys)
            {
                objectivesFilled.Add(key, false);
            }
            balls = data.balls;
            faces = data.faces;

            HasChanged.Invoke(this);
        }

        public void SetSliceBoard(ref SliceBoard slice, Vector3 rotation)
        {

            CubeFace face = CameraController.GetFace(rotation);
            TileData[,] faceTiles = faces.Get((int)face);
            BallData[,] ballData = GetPlaneBoardData(face);
            int faceRotation = GetFaceRotation(face, rotation);

            BoardData data = new BoardData();
            data.balls = ballData.Rotate(faceRotation);
            Assert.IsNotNull(data.balls);
            data.tiles = faceTiles.Rotate(faceRotation);

            slice.rotation = faceRotation;
            slice.face = face;
            slice.SetData(data, objectivesFilled);
        }

        private static int GetFaceRotation(CubeFace face, Vector3 rotation)
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

        private BallData[,] GetPlaneBoardData(CubeFace face)
        {
            FaceModel faceModel = FaceModel.ModelsDictionary[face];

            int[] faceSizes = faceModel.ReorderWithAxes(sizes);
            if (faceModel.mirrorAxis == -1)
                return GetPlaneBoardData(balls, faceSizes, faceModel.inverseZ, faceModel.Getter);
            else
                return GetPlaneBoardData(balls, faceSizes, faceModel.inverseZ, faceModel.Getter).Mirror(faceModel.mirrorAxis);
        }

        private BallData[,] GetPlaneBoardData(BallData[,,] model, int[] sizes, bool inverseZ, Func<BallData[,,], int[], BallData> getter)
        {
            Assert.AreEqual(sizes.Length, 3);
            BallData[,] result = new BallData[sizes[0], sizes[1]];
            for (int first = 0; first < sizes[0]; first++)
            {
                for (int second = 0; second < sizes[1]; second++)
                {
                    int third = 0;
                    while (third < sizes[2])
                    {
                        int iThird = Functions.Inverse(third, sizes[2], inverseZ);

                        BallData ball = getter(model, new int[3] { first, second, iThird });
                        if (ball.BallType != BallType.EMPTY)
                        {
                            result[first, second] = ball;
                            break;
                        }
                        third++;
                    }
                    if (third >= sizes[2])
                    {
                        result[first, second] = BallData.GetEmptyBall();
                    }
                }
            }
            return result;
        }

        //Get the new ball positions from the slice and place the balls to their new positions
        // It relies on the fact that any balltype/objectiveType combiantion is only present once.
        //If a level has two balls exactly similar, this function may not work anymore
        public void SetNewBallPositions(SliceBoard slice)
        {
            Dictionary<BallData, Coords> currentPositions = new Dictionary<BallData, Coords>();
            for (int x = 0; x < X_SIZE; x++)
                for (int y = 0; y < Y_SIZE; y++)
                    for (int z = 0; z < Z_SIZE; z++)
                        if (balls[x, y, z].BallType == BallType.NORMAL)
                            currentPositions.Add(balls[x, y, z], new Coords(x, y, z));

            FaceModel faceModel = FaceModel.ModelsDictionary[slice.face];

            var newPositions = slice.GetBallsPositions();
            var filledTiles = slice.GetFilledTiles();
            foreach (var pair in currentPositions)
            {
                bool wasFilled = filledTiles.Contains(pair.Key.ObjectiveType);
                if (newPositions.ContainsKey(pair.Key) || wasFilled)
                    balls.Set(pair.Value, BallData.GetEmptyBall());
            }
            foreach (var pair in currentPositions)
            {
                Coords newPosition;
                if (newPositions.TryGetValue(pair.Key, out newPosition))
                {
                    Coords realPosition = new Coords();
                    realPosition[faceModel.axes[0]] = newPosition.x;
                    realPosition[faceModel.axes[1]] = newPosition.y;
                    realPosition[faceModel.axes[2]] = pair.Value[faceModel.axes[2]];

                    balls.Set(realPosition, pair.Key);

                }

            }
            if (!CheckLevelCompleted())
            {
                HasChanged.Invoke(this);
            }
        }

        public void ObjectivesFilledNotification(List<ObjectiveType> list)
        {
            foreach(ObjectiveType objective in list)
            {
                bool value;
                if (objectivesFilled.TryGetValue(objective,out value))
                {
                    if (value)
                    {
                        Debug.LogWarning("The model was notified of an objective filling but it was already marked as filled " + objective);
                    }
                    else
                    {
                        objectivesFilled[objective] = true;
                    }
                }
                else
                {
                    Debug.LogError("The model shouldn't be notified of objectives filling that aren't present in the level " + objective);
                }
            }
            CheckLevelCompleted();
        }

        public bool CheckLevelCompleted()
        {
            if (objectivesFilled.AllFilled())
            {
                LevelCompleted.Invoke();
                return true;
            }
            return false;
        }

        void Update()
        {
            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.S))
            {
                CubeLevelData levelData = new CubeLevelData(new CubeData(balls, faces), "CubeLevel1", "CubeLevel1", "CubeLevel2");
                levelData.Save("CubeLevel1", true);
            }
        }
    }
}