﻿using BallMaze.Inputs;
using System;
using UnityEngine;

namespace BallMaze.GameMechanics
{
    class EmptyBall : IBallModel
    {

        private int posX;
        private int posY;

        public event EmptyEventHandler FinishedAnimating;

        public virtual Vector2 GetPosition()
        {
            return new Vector2(posX, posY);
        }

        public virtual void Init(int x, int y, BoardModel model)
        {
            this.posX = x;
            this.posY = y;
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
    }
}