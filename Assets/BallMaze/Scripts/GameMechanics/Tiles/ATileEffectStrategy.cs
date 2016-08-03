namespace BallMaze.GameMechanics.Tiles
{
    internal abstract class ATileEffectStrategy
    {
        protected TileModel tileModel;

        public ATileEffectStrategy(TileModel tile)
        {
            tileModel = tile;
        }

        public abstract void Init();

        public virtual bool ActivateEffect(IBallModel ball)
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

        public virtual void SetState(TileModel.State state)
        {

        }
    }
}

