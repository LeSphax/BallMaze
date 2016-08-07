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
        protected int posZ = 0;

        protected View view;
        protected Board boardModel;
        protected CubeView cube;

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

        public virtual void Init(int x, int y, int z, CubeView cube)
        {
            gameObject.transform.SetParent(cube.transform, false);
            this.cube = cube;
            posX = x;
            posY = y;
            posZ = z;

            InitView(true);
        }

        protected virtual void InitView(bool cube = false)
        {
            
            view.SetPosition(GetWorldPosition(),cube);
        }

        public virtual bool IsEmpty()
        {
            return false;
        }

        public virtual Vector2 GetPosition()
        {
            return new Vector2(posX, posY);
        }



        protected Vector2 GetWorldPosition()
        {

            if (cube != null)
            {
                return cube.GetBallPosition(posX, posY, posZ);
            }
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

        public void SetPosition(Vector3 position)
        {
            transform.localPosition = position;
        }
    }
}