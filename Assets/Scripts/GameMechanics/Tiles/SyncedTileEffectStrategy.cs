
using System.Collections.Generic;
using UnityEngine;
internal class SyncedTileEffectStrategy : ObjectiveTileEffectStrategy
{
    private List<SyncedTileController> _otherSyncedTiles;
    private List<SyncedTileController> otherSyncedTiles
    {
        get
        {
            if (_otherSyncedTiles == null)
            {
                _otherSyncedTiles = new List<SyncedTileController>();
                foreach (GameObject tile in GameObject.FindGameObjectsWithTag(Tags.SyncedTile))
                {
                    if (tile.GetComponent<SyncedTileController>() != null && tile != tileModel.gameObject)
                    {
                        _otherSyncedTiles.Add(tile.GetComponent<SyncedTileController>());
                    }
                }
            }
            return _otherSyncedTiles;
        }
    }
    private bool effectActivated = false;

    public override void Init()
    {
        base.Init();

        tileModel.SetOpen(false);
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
        if (activate)
        {
            effectActivated = true;
            foreach (SyncedTileController tile in otherSyncedTiles)
            {
                tile.SetOpen(true);
            }
        }
        else
        {
            effectActivated = false;
            foreach (SyncedTileController tile in otherSyncedTiles)
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

