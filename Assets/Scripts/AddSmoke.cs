using UnityEngine;
using BallMaze;

public class AddSmoke : MonoBehaviour
{

    void Start()
    {
        GameObject brickSmoke = Resources.Load<GameObject>(Paths.BRICK_SMOKE);
        GameObject dustObject = Instantiate(brickSmoke);
        dustObject.transform.SetParent(transform, false);
    }


}
