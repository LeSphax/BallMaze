using System.Collections.Generic;
using UnityEngine;

namespace BallMaze.GameMechanics
{
    public class SliceBoard : PlayBoard
    {
        public CubeFace face;
        public int rotation
        {
            set;
            private get;
        }

        public void PrintData()
        {
            Debug.Log(boardData);
        }

        protected override bool CheckIfWon()
        {
            return false;
        }

        public Dictionary<BallData, Coords> GetBallsPositions()
        {
            BallData[,] realBalls = new BallData[Width,Height];
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (board[x, y].ball.GetObjectiveType() != ObjectiveType.NONE)
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
            Dictionary<BallData, Coords> result = new Dictionary<BallData, Coords>();
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (realBalls[x, y].BallType == BallType.NORMAL)
                    {
                        Coords point = new Coords(x, y);
                        result.Add(realBalls[x, y], point);
                    }
                }
            }
            return result;
        }
    }
}