namespace BallMaze.GameMechanics.Tiles
{
    internal class NormalTileEffectStrategy : ATileEffectStrategy
    {

        public NormalTileEffectStrategy(TileModel tileModel) : base(tileModel)
        {
        }

        public override void Init()
        {
            tileModel.SetState(TileModel.State.FILLED);
        }
    }

}