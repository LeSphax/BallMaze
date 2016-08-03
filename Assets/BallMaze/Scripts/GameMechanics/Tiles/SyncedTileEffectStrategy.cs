
using System.Collections.Generic;
using UnityEngine;
namespace BallMaze.GameMechanics.Tiles
{
    internal class SyncedTileEffectStrategy : ObjectiveTileEffectStrategy
    {
        private List<SyncedTileModel> otherSyncedTiles;
        private bool effectActivated = false;

        public SyncedTileEffectStrategy(TileModel tile) : base(tile)
        {
        }

        public override void Init()
        {
            base.Init();
            tileModel.SetOpen(false);
            otherSyncedTiles = new List<SyncedTileModel>();
            foreach (GameObject tile in GameObject.FindGameObjectsWithTag(Tags.ObjectiveTile))
            {
                if (tile.GetComponent<SyncedTileModel>() != null && tile != tileModel.gameObject)
                {
                    otherSyncedTiles.Add(tile.GetComponent<SyncedTileModel>());
                }
            }
        }

        public override bool ActivateEffect(IBallModel ball)
        {
            if (ball.GetObjectiveType() == tileModel.GetObjectiveType() && !effectActivated)
            {
                ActivateEffect(true);
                return true;
            }
            else if (effectActivated && ball.GetObjectiveType() != tileModel.GetObjectiveType())
            {
                ActivateEffect(false);
                return true;
            }
            return false;
        }

        public override void ActivateEffect(bool activate)
        {
            if (activate)
            {
                effectActivated = true;
                foreach (SyncedTileModel tile in otherSyncedTiles)
                {
                    tile.SetOpen(true);
                }
            }
            else
            {
                effectActivated = false;
                foreach (SyncedTileModel tile in otherSyncedTiles)
                {
                    tile.SetOpen(false);
                }
            }
        }

        public override bool HasEffect()
        {
            return true;
        }

        public override bool IsEffectActivated()
        {
            return effectActivated;
        }
    }
}

