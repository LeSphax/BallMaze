using BallMaze.Cube;
using BallMaze.Data;
using System.Collections.Generic;
using UnityEngine;

namespace BallMaze.GameMechanics
{
    public delegate void ObjectiveListHandler(List<ObjectiveType> list);

    public class SliceBoard : PlayBoard
    {
        public CubeFace face;

        public int rotation
        {
            set;
            private get;
        }


        protected override bool CheckLevelFinished()
        {
            return false;
        }

        public void SetData(BoardData data, Dictionary<ObjectiveType, bool> filledObjectives)
        {
            base.SetData(data);
            foreach(BoardPosition position in board)
            {
                filledObjectives.TryFillTile(position.tile);
            }
        }

        public List<ObjectiveType> GetFilledTiles()
        {
            List<ObjectiveType> list = new List<ObjectiveType>();
            foreach(BoardPosition position in board)
            {
                ObjectiveType objectiveType = position.tile.GetObjectiveType();
                if (objectiveType != ObjectiveType.NONE && position.tile.IsFilled())
                {
                    Debug.Log(objectiveType + "    " + position.ball.GetPosition());
                    list.Add(objectiveType);
                }
            }
            return list;
        }

        public Dictionary<BallData, IntVector3> GetBallsPositions()
        {
            BallData[,] realBalls = new BallData[Width,Height];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (!board[x, y].ball.IsEmpty() && !board[x, y].ball.IsWall())
                    {
                        realBalls[x, y] = new BallData(BallType.NORMAL, board[x, y].ball.GetObjectiveType());
                    }
                    else
                    {
                        realBalls[x, y] = BallData.GetEmptyBall();
                    }

                }
            }
            FaceModel faceModel = FaceModel.ModelsDictionary[face];
            realBalls = realBalls.Rotate(360 - rotation);

            if (faceModel.mirrorAxis != -1)
                realBalls = realBalls.Mirror(faceModel.mirrorAxis);
            Dictionary<BallData, IntVector3> result = new Dictionary<BallData, IntVector3>();
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (realBalls[x, y].BallType == BallType.NORMAL)
                    {
                        IntVector3 point = new IntVector3(x, y);
                        result.Add(realBalls[x, y], point);
                    }
                }
            }
            return result;
        }
    }
}