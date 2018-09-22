using UnityEngine;

public class PreviousLevelCommand : InputCommand
{

    LevelLoader loader;

    public PreviousLevelCommand()
    {

    }

    public PreviousLevelCommand(LevelLoader loader, SaveManager saveManager = null) : base(saveManager)
    {
        this.loader = loader;
    }

    public override void Execute()
    {
        base.Execute();
        loader.LoadPreviousLevel();
    }

    protected override void PrepareExecution()
    {
        loader = GameObject.FindGameObjectWithTag(Tags.BallMazeController).GetComponent<LevelLoader>();
    }

    public override void LogExecute()
    {
        base.LogExecute();
        GameObject.FindGameObjectWithTag(Tags.PreviousLevelButton).GetComponent<BlinkingButton>().BlinkOnce();
    }
}
