using BallMaze.Inputs;
using System;
using UnityEngine;
namespace BallMaze.GameMechanics
{
    [Serializable]
    public class BallModel : MonoBehaviour, IBallModel
    {

        protected int posX;
        protected int posY;

        protected BoardModel boardModel;

        public ObjectiveType objectiveType = ObjectiveType.NONE;

        public event EmptyEventHandler FinishedAnimating;

        public ObjectiveType GetObjectiveType()
        {
            return objectiveType;
        }

        protected void RaiseFinishedAnimating()
        {
            if (FinishedAnimating != null)
                FinishedAnimating.Invoke();
            else
                Debug.LogError("The brickModel should be listened to");

        }

        public virtual void Move(Direction direction)
        {
        }

        public virtual void MoveBack(int oldPosX, int oldPosY)
        {
        }

        public override string ToString()
        {
            return "Wall";
        }

        public virtual bool IsMoving()
        {
            return false;
        }

        public virtual void Init(int x, int y, BoardModel boardModel)
        {
            gameObject.transform.SetParent(boardModel.transform, false);
            this.boardModel = boardModel;
            posX = x;
            posY = y;

            InitView();
        }

        protected virtual void InitView()
        {
            BallView view = gameObject.GetComponent<BallView>();
            view.SetPosition(GetWorldPosition());
        }

        public virtual bool IsEmpty()
        {
            return false;
        }

        public virtual Vector2 GetPosition()
        {
            return new Vector2(posX, posY);
        }



        protected Vector3 GetWorldPosition()
        {
            return boardModel.GetWorldPosition(posX, posY);
        }

        public virtual void FillObjective()
        {
        }

        public virtual void UnFillObjective()
        {
        }

        public virtual void InitObjectiveType(ObjectiveType type)
        {
            objectiveType = type;
        }

        public virtual void Destroy()
        {
            Destroy(gameObject);
        }
    }
}