using UnityEngine;

public abstract class GameObjectWithMesh : MonoBehaviour
{
    [SerializeField]
    private GameObject _Mesh;
    public virtual GameObject Mesh
    {
        get
        {
            return _Mesh;
        }
        set
        {
            _Mesh = value;
        }
    }
}
