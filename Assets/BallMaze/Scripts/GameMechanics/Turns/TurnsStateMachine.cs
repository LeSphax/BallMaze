using GenericStateMachine;
using UnityEngine;

namespace BallMaze.GameMechanics.Turns
{
    internal class TurnsStateMachine : StateMachine<TurnsStateMachine, TurnEvent>
    {
        internal Turn turn;

        public TurnsStateMachine(Turn turn) : base()
        {
            this.turn = turn;
        }

        internal override State<TurnsStateMachine, TurnEvent> DefineFirst()
        {
            return new Created(this);
        }
    }
    internal abstract class TurnState : State<TurnsStateMachine, TurnEvent>
    {
        public TurnState(TurnsStateMachine stateMachine) : base(stateMachine)
        {
        }
    }

    internal class Created : State<TurnsStateMachine, TurnEvent>
    {
        public Created(TurnsStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void enter()
        {
            Debug.Log(stateMachine.turn);
        }
    }

    internal class Moving : State<TurnsStateMachine, TurnEvent>
    {
        public Moving(TurnsStateMachine stateMachine) : base(stateMachine)
        {
        }
    }
    internal class Filling : State<TurnsStateMachine, TurnEvent>
    {
        public Filling(TurnsStateMachine stateMachine) : base(stateMachine)
        {
        }
    }
    internal class Activating : State<TurnsStateMachine, TurnEvent>
    {
        public Activating(TurnsStateMachine stateMachine) : base(stateMachine)
        {
        }
    }

    internal class Played : State<TurnsStateMachine, TurnEvent>
    {
        public Played(TurnsStateMachine stateMachine) : base(stateMachine)
        {
        }
    }

    internal class UnMoving : State<TurnsStateMachine, TurnEvent>
    {
        public UnMoving(TurnsStateMachine stateMachine) : base(stateMachine)
        {
        }
    }
    internal class UnFilling : State<TurnsStateMachine, TurnEvent>
    {
        public UnFilling(TurnsStateMachine stateMachine) : base(stateMachine)
        {
        }
    }
    internal class UnActivating : State<TurnsStateMachine, TurnEvent>
    {
        public UnActivating(TurnsStateMachine stateMachine) : base(stateMachine)
        {
        }
    }


}
