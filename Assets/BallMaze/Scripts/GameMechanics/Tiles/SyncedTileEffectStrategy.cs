
using System.Collections.Generic;
using UnityEngine;
namespace BallMaze.GameMechanics.Tiles
{
    internal class SyncedTileEffectStrategy : ObjectiveTileEffectStrategy
    {
        private List<SyncedTileModel> otherSyncedTiles;
        private bool effectActivated = false;

        public override void Init()
        {
            base.Init();
            
            tileModel.SetOpen(false);
        }

        void Start()
        {
            otherSyncedTiles = new List<SyncedTileModel>();
            foreach (GameObject tile in GameObject.FindGameObjectsWithTag(Tags.SyncedTile))
            {
                if (tile.GetComponent<SyncedTileModel>() != null && tile != tileModel.gameObject)
                {
                    otherSyncedTiles.Add(tile.GetComponent<SyncedTileModel>());
                }
            }
        }

        public override bool ActivateEffect(IBallController ball)
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
            Debug.Log(otherSyncedTiles.Count);
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

