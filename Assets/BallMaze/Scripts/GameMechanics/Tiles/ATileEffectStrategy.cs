using UnityEngine;

namespace BallMaze.GameMechanics.Tiles
{
    public abstract class ATileEffectStrategy : MonoBehaviour
    {
        protected TileController tileModel;

        void Awake()
        {
            tileModel = GetComponent<TileController>();
        }

        public abstract void Init();

        public virtual bool ActivateEffect(IBallController ball)
        {
            return false;
        }

        public virtual void ActivateEffect(bool activate)
        {
        }

        public virtual bool HasEffect()
        {
            return false;
        }

        public virtual bool IsEffectActivated()
        {
            return false;
        }

        public virtual void SetState(TileController.State state)
        {

        }
    }
}

