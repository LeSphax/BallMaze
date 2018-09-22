using UnityEngine;

internal class ObjectiveTileEffectStrategy : ATileEffectStrategy
{
    private ParticleSystem.EmissionModule dust;
    private GameObject dustObject;

    public override void Init()
    {
        GameObject fairyDust = Resources.Load<GameObject>(Paths.FAIRY_DUST);
        dustObject = Object.Instantiate(fairyDust);
        dustObject.transform.SetParent(tileModel.transform, false);
        dust = dustObject.GetComponent<ParticleSystem>().emission;
        tileModel.SetOpen(true);
    }

    public override void SetState(TileController.State newState)
    {
        switch (newState)
        {
            case TileController.State.CLOSED:
                dust.enabled = false;
                break;
            case TileController.State.OPEN:
                dust.enabled = true;
                break;
            case TileController.State.FILLED:
                dust.enabled = false;
                break;
            default:
                break;
        }
    }

}

