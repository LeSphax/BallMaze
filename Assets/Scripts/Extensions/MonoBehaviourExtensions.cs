
using UnityEngine;

public static class MonoBehaviourExtensions
{
    public static GameObject InstantiateAsChildren(this MonoBehaviour monoBehaviour, GameObject prefab)
    {
        GameObject gameObject = Object.Instantiate(prefab);
        gameObject.transform.SetParent(monoBehaviour.transform, false);
        return gameObject;
    }
}

