using UnityEngine;

namespace BallMaze.GameMechanics.Tiles
{
    internal class ObjectiveTileEffectStrategy : ATileEffectStrategy
    {
        private ParticleSystem.EmissionModule dust;
        private GameObject dustObject;

        public ObjectiveTileEffectStrategy(TileModel tile) : base(tile)
        {
        }

        public override void Init()
        {
            tileModel.SetOpen(true);
            GameObject fairyDust = Resources.Load<GameObject>(Paths.FAIRY_DUST);
            dustObject = Object.Instantiate(fairyDust);
            dustObject.transform.SetParent(tileModel.transform, false);
            dust = dustObject.GetComponent<ParticleSystem>().emission;
        }

        public override void SetState(TileModel.State newState)
        {
            switch (newState)
            {
                case TileModel.State.CLOSED:
                    dust.enabled = false;
                    break;
                case TileModel.State.OPEN:
                    dust.enabled = true;
                    break;
                case TileModel.State.FILLED:
                    dust.enabled = false;
                    break;
                default:
                    break;
            }
        }

    }

}