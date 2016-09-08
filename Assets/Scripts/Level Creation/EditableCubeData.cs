using BallMaze.Cube;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace BallMaze.Cube
{
    public class EditableCubeData : CubeData
    {
        private const int XSize_MAX = 10;
        private const int YSize_MAX = 10;
        internal CubeController cubeController;

        private TileData[][,] oldFaces = new TileData[6][,];
        private BallData[,,] oldBalls = new BallData[0, 0, 0];

        public EditableCubeData()
        {

        }

        internal EditableCubeData(CubeController controller)
        {
            cubeController = controller;

        }

        public void DestroyBoard()
        {
            SaveBoard();
            ResetBoard();
        }

        private void SaveBoard()
        {
            oldFaces = faces;
            oldBalls = balls;
        }

        public void ResetBoard()
        {
            faces = TileData.GetEmptyFaceArray(new int[] { 0, 0, 0 });
            balls = BallData.GetEmptyBallDataMatrix(0, 0, 0);
        }

        public void CreateBoard(int XSize, int YSize, int ZSize)
        {
            Assert.IsTrue(oldFaces.GetLength(0) == oldBalls.GetLength(0) && oldFaces.GetLength(1) == oldBalls.GetLength(1));
            faces = TileData.GetEmptyFaceArray(new int[] { XSize, YSize, ZSize }, oldFaces);
            balls = InitBalls(XSize, YSize, ZSize);
            InitBalls(XSize, YSize, ZSize);
            UpdateModel();
        }

        private BallData[,,] InitBalls(int XSize, int YSize, int ZSize)
        {
            BallData[,,] result = new BallData[XSize, YSize, ZSize];
            for (int i = 0; i < XSize; i++)
            {
                for (int j = 0; j < YSize; j++)
                {
                    for (int k = 0; k < ZSize; k++)
                    {
                        if (oldBalls.GetLength(0) > i && oldBalls.GetLength(1) > j && oldBalls.GetLength(2) > k)
                        {
                            balls[i, j, k] = oldBalls[i, j, k];
                        }
                        else
                        {
                            balls[i, j, k] = BallData.GetEmptyBall();
                        }
                    }
                }
            }
            return result;
        }

        public void NextBall(int posX, int posY, int posZ)
        {
            balls[posX, posY, posZ] = balls[posX, posY, posZ].GetNext();
            UpdateModel();
        }

        public void NextTileObjective(int face, int posX, int posY)
        {
            if (!TileExists(face, posX, posY))
                faces[face][posX, posY].ObjectiveType = faces[face][posX, posY].ObjectiveType.Next();
            UpdateModel();
        }

        public void NextTileType(int face, int posX, int posY)
        {
            if (!TileExists(face, posX, posY))
                faces[face][posX, posY].TileType = faces[face][posX, posY].TileType.Next();
            UpdateModel();
        }

        public void PreviousTileObjective(int face, int posX, int posY)
        {
            if (!TileExists(face, posX, posY))
                faces[face][posX, posY].ObjectiveType = faces[face][posX, posY].ObjectiveType.Previous();
            UpdateModel();
        }

        public void PreviousTileType(int face, int posX, int posY)
        {
            if (!TileExists(face, posX, posY))
                faces[face][posX, posY].TileType = faces[face][posX, posY].TileType.Previous();
            UpdateModel();
        }

        internal void SetData(CubeData cubeData)
        {
            faces = cubeData.faces;
            balls = cubeData.balls;
            UpdateModel();
        }

        public bool TileExists(int face, int posX, int posY)
        {
            if (faces[face][posX, posY] == null)
            {
                return true;
            }
            return false;
        }

        internal void PreviousBall(int posX, int posY, int posZ)
        {
            balls[posX, posY, posZ] = balls[posX, posY, posZ].GetPrevious();
            UpdateModel();
        }

        private void UpdateModel()
        {
            cubeController.SetData(this);
        }
    }
}