using UnityEngine;

namespace BallMaze.GameMechanics.Tiles
{
    class SyncedTileModel : TileController
    {

        private const float grey = 174.0f / 255;

        public override void SetMesh(GameObject mesh)
        {
            base.SetMesh(mesh);
            view.Color = new Color(grey, grey, grey);
        }

        protected override void InitEffectManager()
        {
            effectManager = gameObject.AddComponent<SyncedTileEffectStrategy>();
            effectManager.Init();
        }
    }
}

