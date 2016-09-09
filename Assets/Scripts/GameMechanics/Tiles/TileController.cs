using CustomAnimations.BallMazeAnimations;
using System;
using UnityEngine;

namespace BallMaze.GameMechanics.Tiles
{
    [Serializable]
    public class TileController : MonoBehaviour
    {
        protected TileView view;
        protected ATileEffectStrategy effectManager;

        public GameObject Mesh
        {
            get
            {
                return view.Mesh;
            }
        }

        public bool IsOpen
        {
            get
            {
                return state == State.OPEN;
            }
        }

        public enum State
        {
            CLOSED,
            OPEN,
            FILLED,
        }

        private State state;
        protected ObjectiveType objectiveType;
        public TileType tileType;

        protected virtual void InitEffectManager()
        {
            if (objectiveType == ObjectiveType.OBJECTIVE1 || objectiveType == ObjectiveType.OBJECTIVE2)
                if (tileType == TileType.NORMAL)
                    effectManager = gameObject.AddComponent<ObjectiveTileEffectStrategy>();
                else
                    effectManager = gameObject.AddComponent<SyncedTileEffectStrategy>();
            else
                effectManager = gameObject.AddComponent<NormalTileEffectStrategy>();

            effectManager.Init();
        }

        public void SetOpen(bool open)
        {
            if (open)
            {
                SetState(State.OPEN);
            }
            else
            {
                SetState(State.CLOSED);
            }
        }

        internal ColorAnimation GetFillingAnimation(float duration)
        {
            return view.GetFillingAnimation(duration);
        }

        public void Fill(bool fill = true)
        {
            if (fill)
                GetFillingAnimation(0).StartAnimating();
            else
                GetFillingAnimation(0).StartReverseAnimating();
        }

        public ObjectiveType GetObjectiveType()
        {
            return objectiveType;
        }

        public void SetState(State newState)
        {
            effectManager.SetState(newState);
            state = newState;
        }

        public bool TryFillTile()
        {
            if (IsOpen)
            {
                SetState(State.FILLED);
                return true;
            }
            return false;
        }

        public void UnFillTile()
        {
            SetState(State.OPEN);
        }

        public bool ActivateEffect(IBallController ball)
        {
            return effectManager.ActivateEffect(ball);
        }

        public void ActivateEffect(bool activate)
        {
            effectManager.ActivateEffect(activate);
        }

        public bool HasEffect()
        {
            return effectManager.HasEffect();
        }

        public bool IsEffectActivated()
        {
            return effectManager.IsEffectActivated();
        }

        public bool IsFilled()
        {
            return state == State.FILLED;
        }

        public void Init(TileData tileData)
        {
            objectiveType = tileData.ObjectiveType;
            tileType = tileData.TileType;
            InitEffectManager();
        }

        public virtual void SetMesh(GameObject mesh)
        {
            gameObject.name = mesh.name;
            mesh.transform.SetParent(transform, false);
            view.Mesh = mesh;
        }

        void Awake()
        {
            view = gameObject.AddComponent<TileView>();
        }
    }

}