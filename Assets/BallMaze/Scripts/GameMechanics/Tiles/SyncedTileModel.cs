namespace BallMaze.GameMechanics.Tiles
{
    class SyncedTileModel : TileModel
    {

        protected override void InitEffectManager()
        {
            effectManager = new SyncedTileEffectStrategy(this);
            effectManager.Init();
        }
    }
}

