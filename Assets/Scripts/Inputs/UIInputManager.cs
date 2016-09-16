using BallMaze.Inputs;
using UnityEngine;

public class UIInputManager : MonoBehaviour {

    public InputManager manager;

    void Start()
    {
        manager = GameObjects.GetInputManager();
    }

	public void Cancel()
    {
        manager.Cancel();
    }

    public void Reset()
    {
        manager.Reset();
    }

    public void PreviousLevel()
    {
        manager.PreviousLevel();
    }

    public void Quit()
    {
        manager.Quit();
    }

}
