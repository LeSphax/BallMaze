using BallMaze.Inputs;
using UnityEngine;

public class CreateInputManager : MonoBehaviour
{

    // Use this for initialization
    void Awake()
    {
#if UNITY_EDITOR
        gameObject.AddComponent<PCInputManager>();
#else
        if (Platform.Mobile())
        {
            gameObject.AddComponent<MobileInputManager>();
        }
        else
        {
            gameObject.AddComponent<PCInputManager>();
        }
#endif
    }

}
