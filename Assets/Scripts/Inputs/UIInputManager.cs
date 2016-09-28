using BallMaze.Inputs;
using UnityEngine;

public class UIInputManager : MonoBehaviour {

    private InputManager manager;

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

    public void ReloadLevel()
    {
        manager.Reload();
    }

    public void PreviousLevel()
    {
        manager.PreviousLevel();
    }

    public void NextLevel()
    {
        manager.NextLevel();
    }

    public void Quit()
    {
        manager.Quit();
    }

}
