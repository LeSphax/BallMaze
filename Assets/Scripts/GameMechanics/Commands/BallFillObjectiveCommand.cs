using UnityEngine;

internal class BallFillObjectiveCommand : AbstractBallCommand
{

    private IBallController ball;
    private TileController tile;
    private Turn turn;
    private bool wasUseful = false;

    public BallFillObjectiveCommand(Turn turn, IBallController ball, TileController tile)
    {
        this.ball = ball;
        this.tile = tile;
        this.turn = turn;
    }

    public override void Execute()
    {
        if (tile.TryFillTile())
        {
            turn.objectivesFilled.Add(ball.GetObjectiveType());
            wasUseful = true;
            ball.FinishedAnimating += new EmptyEventHandler(RaiseFinishedExecuting);
            //GameObject.FindGameObjectWithTag(Tags.LevelController).GetComponent<LevelManager>().NotifyFilledObjective(tile.GetObjectiveType());
            ball.FillObjective();
        }
        else
        {
            RaiseFinishedExecuting();
        }
    }

    protected override void ExecuteUndo()
    {
        ball.FinishedAnimating += new EmptyEventHandler(RaiseFinishedExecuting);
        GameObject.FindGameObjectWithTag(Tags.LevelController).GetComponent<LevelManager>().NotifyUnFilledObjective(ball.GetObjectiveType());
        tile.UnFillTile();
        ball.UnFillObjective();
    }

    protected override void TakeOffListener()
    {
        ball.FinishedAnimating -= new EmptyEventHandler(RaiseFinishedExecuting);
    }

    public override bool WasUseful()
    {
        return wasUseful;
    }
}

