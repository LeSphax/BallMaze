using System.Xml.Serialization;
using UnityEngine;

public delegate void ReceiveBoardInput(BoardInputCommand command);

[XmlInclude(typeof(ResetCommand))]
[XmlInclude(typeof(MoveCommand))]
[XmlInclude(typeof(CancelCommand))]
public abstract class BoardInputCommand : InputCommand
{

    protected PlayBoard model;

    public BoardInputCommand(SaveManager saveManager = null) : base(saveManager)
    {
    }

    protected override void PrepareExecution()
    {
        model = GameObject.FindGameObjectWithTag(Tags.LevelController).GetComponent<PlayBoard>();
    }

    public void SetModel(PlayBoard model)
    {
        this.model = model;
    }

    public override void LogExecute()
    {
        PrepareExecution();
        saveManager = null;
        model.ReceiveInputCommand(this);
    }

    //public abstract void Undo();

    //public abstract bool IsExecuting();
}
