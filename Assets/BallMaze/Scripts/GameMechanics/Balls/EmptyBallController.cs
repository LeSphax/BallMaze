using BallMaze.Inputs;
using System;
using UnityEngine;

namespace BallMaze.GameMechanics
{
    class EmptyBallController : IBallController
    {

        private int posX;
        private int posY;
        private int posZ;

        public event EmptyEventHandler FinishedAnimating;

        public virtual Vector3 GetPosition()
        {
            return new Vector3(posX, posY, posZ);
        }

        public virtual void Init(int x, int y, int z, Board model)
        {
            this.posX = x;
            this.posY = y;
            posZ = z;
        }

        public virtual void Init(int x, int y, int z, CubeModel model)
        {
            this.posX = x;
            this.posY = y;
            posZ = z;
        }

        public virtual bool IsEmpty()
        {
            return true;
        }

        public virtual void Move(Direction direction)
        {
            throw new NotImplementedException();
        }

        public virtual void MoveBack(int oldPosX, int oldPosY)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return "EmptyBrick";
        }

        public virtual void FillObjective()
        {
            throw new NotImplementedException();
        }

        public virtual void UnFillObjective()
        {
            throw new NotImplementedException();
        }

        public virtual bool IsMoving()
        {
            return false;
        }

        public virtual void InitObjectiveType(ObjectiveType type)
        {
        }

        public void Destroy()
        {
        }

        public ObjectiveType GetObjectiveType()
        {
            return ObjectiveType.NONE;
        }

        public void SetMesh(GameObject mesh)
        {
            throw new Exception("Empty balls don't have a mesh");
        }
    }
}