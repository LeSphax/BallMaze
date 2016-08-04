using BallMaze.Inputs;
using System;
using UnityEngine;
namespace BallMaze.GameMechanics
{
    [Serializable]
    public abstract class BallController<View> : MonoBehaviour, IBallController where View : BallView
    {

        protected int posX;
        protected int posY;

        protected View view;
        protected Board boardModel;

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

        protected virtual void Awake()
        {
            view = gameObject.AddComponent<View>();
        }

        public virtual void Init(int x, int y, Board boardModel)
        {
            gameObject.transform.SetParent(boardModel.transform, false);
            this.boardModel = boardModel;
            posX = x;
            posY = y;

            InitView();
        }

        protected virtual void InitView()
        {
            
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

        public void SetMesh(GameObject mesh)
        {
            gameObject.name = mesh.name;
            mesh.transform.SetParent(transform);
            view.Mesh = mesh;
        }
    }
}