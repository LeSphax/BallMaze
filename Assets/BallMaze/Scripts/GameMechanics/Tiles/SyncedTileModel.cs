namespace BallMaze.GameMechanics.Tiles
{
    class SyncedTileModel : TileController
    {

        protected override void InitEffectManager()
        {
            effectManager = gameObject.AddComponent<SyncedTileEffectStrategy>();
            effectManager.Init();
        }
    }
}

