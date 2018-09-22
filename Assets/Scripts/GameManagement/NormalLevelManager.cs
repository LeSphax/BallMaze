

using UnityEngine;

public class NormalLevelManager : LevelManager
{

    public ObjectiveType[] objectivesOrder;
    private int objectivesFilled = 0;

    private GameObject resetButton;
    private bool waitingForInput = false;

    void Awake()
    {
        resetButton = GameObject.FindGameObjectWithTag(Tags.ResetButton);
        GetComponent<PlayBoard>().LevelFinished += LevelFinished;
    }

    void Start()
    {
        GameObjects.GetInputManager().AnyInput += EndLevelIfFinished;
    }

    internal override void NotifyFilledObjective(ObjectiveType objectiveEntered)
    {
        if (objectivesFilled < objectivesOrder.Length && objectivesOrder[objectivesFilled] != objectiveEntered && objectivesOrder[objectivesFilled] != ObjectiveType.NONE)
        {
            Invoke("StartBlinking", 8.0f);
        }
        objectivesFilled++;
    }

    internal override void NotifyUnFilledObjective(ObjectiveType objectiveType)
    {
        objectivesFilled--;
        StopBlinking();
    }

    private void StopBlinking()
    {
        CancelInvoke();
        resetButton.SendMessage("StopBlinking");
    }

    internal override void SetObjectiveOrder(ObjectiveType firstObjective)
    {
        objectivesOrder = new ObjectiveType[1];
        objectivesOrder[0] = firstObjective;
    }

    void StartBlinking()
    {
        resetButton.SendMessage("Blink");
    }

    internal override void LevelFinished()
    {
        GameObjects.GetLevelLoader().LoadNextLevelDelayed();

        //foreach (GameObject tile in Tags.FindObjectiveTiles())
        //{
        //    //tile.AddComponent<WinAnim>();
        //    //CameraFade.StartFade(CameraFade.FadeType.FADEIN, Color.white * new Vector4(1, 1, 1, 0.5f));
        //}
        //waitingForInput = true;
    }

    private void EndLevelIfFinished()
    {
        if (waitingForInput)
            GameObjects.GetLevelLoader().LoadNextLevelDelayed();
    }

}
