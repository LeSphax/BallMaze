using BallMaze.GameMechanics.Commands;
using BallMaze.GameMechanics.Tiles;
using BallMaze.Inputs;
using System;
using System.Collections.Generic;

namespace BallMaze.GameMechanics.Turns
{
    class Turn
    {
        public event EmptyEventHandler FinishedPlaying;
        private enum State
        {
            CREATED,
            MOVING,
            FILLING,
            ACTIVATING,
            UNFILLING,
            UNMOVING,
            PLAYED,
            FINISHED,
            UNACTIVATING,
        }

        private State state;
        private BoardModel model;
        private Stack<BallMoveCommand> moveCommands;
        private Stack<BallFillObjectiveCommand> objectiveCommands;
        private Stack<EffectActivationCommand> effectActivationCommands;
        private List<AbstractBallCommand> executingCommands;

        public Turn(BoardModel model)
        {
            state = State.CREATED;
            this.model = model;
            executingCommands = new List<AbstractBallCommand>();
            moveCommands = new Stack<BallMoveCommand>();
            objectiveCommands = new Stack<BallFillObjectiveCommand>();
            effectActivationCommands = new Stack<EffectActivationCommand>();
        }

        public void Play(Direction direction)
        {
            if (state == State.CREATED)
            {
                state = State.MOVING;
                if (direction == Direction.UP)
                {
                    for (int y = model.Height - 1; y >= 0; y--)
                    {
                        for (int x = 0; x < model.Width; x++)
                        {
                            MoveBrick(direction, x, y);
                        }
                    }
                }
                else if (direction == Direction.DOWN)
                {
                    for (int y = 0; y < model.Height; y++)
                    {
                        for (int x = 0; x < model.Width; x++)
                        {
                            MoveBrick(direction, x, y);
                        }
                    }
                }
                else if (direction == Direction.RIGHT)
                {
                    for (int x = model.Width - 1; x >= 0; x--)
                    {
                        for (int y = 0; y < model.Height; y++)
                        {
                            MoveBrick(direction, x, y);
                        }
                    }
                }
                else if (direction == Direction.LEFT)
                {
                    for (int x = 0; x < model.Width; x++)
                    {
                        for (int y = 0; y < model.Height; y++)
                        {

                            MoveBrick(direction, x, y);
                        }
                    }
                }
                ExecuteCommands();
            }
            else
            {
                throw new Exception("A turn can only be played once");
            }

        }

        internal void UnPlay()
        {
            if (state != State.PLAYED)
            {
                throw new Exception("State : " + state + " Can't undo a turn before it has been played");
            }
            else
            {
                if (objectiveCommands.Count > 0)
                {
                    state = State.UNFILLING;
                    UnFillObjectives();
                }
                else if (effectActivationCommands.Count > 0)
                {
                    state = State.UNACTIVATING;
                    UnActivateEffects();
                }
                else if (moveCommands.Count > 0)
                {
                    state = State.UNMOVING;
                    UnMoveBricks();
                }
            }
        }

        private void UnFillObjectives()
        {
            while (objectiveCommands.Count > 0)
            {
                AbstractBallCommand command = objectiveCommands.Pop();
                executingCommands.Add(command);
            }
            UndoCommands();
        }

        private void UnActivateEffects()
        {
            while (effectActivationCommands.Count > 0)
            {
                AbstractBallCommand command = effectActivationCommands.Pop();
                executingCommands.Add(command);
            }
            UndoCommands();
        }

        private void UnMoveBricks()
        {
            while (moveCommands.Count > 0)
            {
                AbstractBallCommand command = moveCommands.Pop();
                executingCommands.Add(command);
            }
            UndoCommands();
        }

        private void MoveBrick(Direction direction, int x, int y)
        {
            if (!model.IsTileEmpty(x, y) && model.GetBrick(x, y).IsMoving())
            {
                BallMoveCommand moveCommand = new BallMoveCommand(model.GetBrick(x, y), direction);
                PrepareBrickMove(moveCommand);
            }
        }

        private void PrepareBrickMove(BallMoveCommand command)
        {
            PrepareCommand(command);
            moveCommands.Push(command);
        }

        private void PrepareBrickFillObjective(BallFillObjectiveCommand command)
        {
            PrepareCommand(command);
            objectiveCommands.Push(command);
        }

        private void PrepareEffectActivation(EffectActivationCommand command)
        {
            PrepareCommand(command);
            effectActivationCommands.Push(command);
        }

        private void PrepareCommand(AbstractBallCommand command)
        {
            command.FinishedExecuting += new BallCommandEventHandler(FinishedExecutingCommand);
            executingCommands.Add(command);
        }

        private void ExecuteCommands()
        {
            if (executingCommands.Count == 0)
            {
                CommandsFinished();
            }
            else
            {
                List<AbstractBallCommand> clone = new List<AbstractBallCommand>(executingCommands);
                for (int i = 0; i < clone.Count; i++)
                {
                    clone[i].Execute();
                }
            }
        }

        private void UndoCommands()
        {
            if (executingCommands.Count == 0)
            {
                CommandsFinished();
            }
            else
            {
                List<AbstractBallCommand> clone = new List<AbstractBallCommand>(executingCommands);
                for (int i = 0; i < clone.Count; i++)
                {
                    clone[i].Undo();
                }
            }
        }

        public void FinishedExecutingCommand(AbstractBallCommand command)
        {
            executingCommands.Remove(command);
            CheckIfCommandsLeft();
        }

        private void CheckIfCommandsLeft()
        {
            if (executingCommands.Count == 0)
            {
                CommandsFinished();
            }
        }

        private void CommandsFinished()
        {
            switch (state)
            {
                case State.MOVING:
                    state = State.ACTIVATING;
                    if (AddActivations())
                    {
                        ExecuteCommands();
                    }
                    else
                    {
                        CommandsFinished();
                    }
                    break;
                case State.ACTIVATING:
                    state = State.FILLING;
                    if (AddObjectivesToFill())
                    {
                        ExecuteCommands();
                    }
                    else
                    {
                        CommandsFinished();
                    }
                    break;
                case State.FILLING:
                    state = State.PLAYED;
                    FinishedPlaying.Invoke();
                    break;
                case State.UNFILLING:
                    state = State.UNACTIVATING;
                    UnActivateEffects();
                    break;
                case State.UNACTIVATING:
                    state = State.UNMOVING;
                    UnMoveBricks();
                    break;
                case State.UNMOVING:
                    state = State.FINISHED;
                    FinishedPlaying.Invoke();
                    break;
                default:
                    throw new Exception("State :" + state + " - Can't finish commands if no turn was playing");
            }
        }

        private bool AddActivations()
        {
            for (int x = 0; x < model.Width; x++)
            {
                for (int y = 0; y < model.Height; y++)
                {
                    IBallModel ball = model.GetBrick(x, y);
                    TileModel tile = model.GetTile(x, y);
                    if (tile.HasEffect())
                    {
                        EffectActivationCommand command = new EffectActivationCommand(ball, tile);
                        PrepareEffectActivation(command);
                    }
                }
            }
            return effectActivationCommands.Count > 0;
        }

        private bool AddObjectivesToFill()
        {
            for (int x = 0; x < model.Width; x++)
            {
                for (int y = 0; y < model.Height; y++)
                {
                    IBallModel ball = model.GetBrick(x, y);
                    TileModel tile = model.GetTile(x, y);
                    if (ball.GetObjectiveType() != ObjectiveType.NONE && ball.GetObjectiveType() == tile.GetObjectiveType())
                    {
                        BallFillObjectiveCommand command = new BallFillObjectiveCommand(ball, tile);
                        PrepareBrickFillObjective(command);
                    }
                }
            }
            return objectiveCommands.Count > 0;
        }

        public bool WasUseful()
        {
            foreach (AbstractBallCommand command in moveCommands)
            {
                if (command.WasUseful())
                {
                    return true;
                }
            }

            foreach (AbstractBallCommand command in objectiveCommands)
            {
                if (command.WasUseful())
                {
                    return true;
                }
            }

            foreach (AbstractBallCommand command in effectActivationCommands)
            {
                if (command.WasUseful())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
