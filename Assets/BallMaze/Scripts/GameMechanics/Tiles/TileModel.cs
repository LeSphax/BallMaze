using System;
using UnityEngine;

namespace BallMaze.GameMechanics.Tiles
{
    [Serializable]
    internal class TileModel : MonoBehaviour
    {

        protected ATileEffectStrategy effectManager;

        public bool IsOpen
        {
            get
            {
                return state == State.OPEN;
            }
        }

        public Color Color
        {
            get
            {
                return GetComponent<Renderer>().material.color;
            }
            internal set
            {
                GetComponent<Renderer>().material.color = value;
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
                    effectManager = new ObjectiveTileEffectStrategy(this);
                else
                    effectManager = new SyncedTileEffectStrategy(this);
            else
                effectManager = new NormalTileEffectStrategy(this);

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

        public bool ActivateEffect(IBallModel ball)
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
        }

        void Start()
        {
            InitEffectManager();
        }
    }

}