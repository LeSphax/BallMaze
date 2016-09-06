using GenericStateMachine;
using UnityEngine;
using UnityEngine.Assertions;

namespace BallMaze.GameMechanics.ObjectiveBall
{

    internal abstract class FinishedAnimationTransition : Transition<BallViewStateMachine, BallViewEvent, FinishedAnimation>
    {
        public FinishedAnimationTransition(BallViewState state) : base(state)
        {
        }

        public override void action(FinishedAnimation evt)
        {
            Object.Destroy(evt.animation);
        }
    }

    internal class CompletedTransition : Transition<BallViewStateMachine, BallViewEvent, FinishedAnimation>
    {
        public CompletedTransition(BallViewState state) : base(state)
        {
        }

        public override State<BallViewStateMachine, BallViewEvent> goTo()
        {
            return new Completed(myState.stateMachine);
        }
    }

    internal class ReturnToIdleTransition : FinishedAnimationTransition
    {
        public ReturnToIdleTransition(BallViewState state) : base(state)
        {
        }

        public override State<BallViewStateMachine, BallViewEvent> goTo()
        {
            return new Idle(myState.stateMachine);
        }

    }

    internal class MoveAfterAnimationTransition : FinishedAnimationTransition
    {
        Vector3 moveTarget;

        public MoveAfterAnimationTransition(BallViewState state, Vector3 moveTarget) : base(state)
        {
            this.moveTarget = moveTarget;
        }

        public override void action(FinishedAnimation evt)
        {
            myState.stateMachine.view.StartMovingTowards(moveTarget);
        }

        public override State<BallViewStateMachine, BallViewEvent> goTo()
        {
            return new Moving(myState.stateMachine);
        }
    }

    internal class StockMoveTransition : Transition<BallViewStateMachine, BallViewEvent, MoveCommand>
    {
        public StockMoveTransition(BallViewState state) : base(state)
        {
        }

        public override void action(MoveCommand evt)
        {
            #if !UNITY_WP_8_1
            Assert.IsTrue(myState.GetType().IsSubclassOf(typeof(BallViewState)));
#endif
            BallViewState state = (BallViewState)myState;
            new MoveAfterAnimationTransition(state, evt.target);
        }

        public override State<BallViewStateMachine, BallViewEvent> goTo()
        {
            return myState;
        }
    }

    internal class MoveTransition : Transition<BallViewStateMachine, BallViewEvent, MoveCommand>
    {
        public MoveTransition(BallViewState state) : base(state)
        {
        }

        public override void action(MoveCommand evt)
        {
            myState.stateMachine.view.StartMovingTowards(evt.target);
        }

        public override State<BallViewStateMachine, BallViewEvent> goTo()
        {
            return new Moving(myState.stateMachine);
        }
    }


    internal class CompletingTransition : Transition<BallViewStateMachine, BallViewEvent, CompleteCommand>
    {
        public CompletingTransition(BallViewState state) : base(state)
        {
        }

        public override void action(CompleteCommand evt)
        {
            myState.stateMachine.view.StartCompletingAnimation(evt.objective);
        }

        public override State<BallViewStateMachine, BallViewEvent> goTo()
        {
            return new Completed(myState.stateMachine);
        }
    }

    internal class UnCompletingTransition : Transition<BallViewStateMachine, BallViewEvent, CompleteCommand>
    {
        public UnCompletingTransition(BallViewState state) : base(state)
        {
        }

        public override void action(CompleteCommand evt)
        {
            myState.stateMachine.view.StartUncompletingAnimation(evt.objective);
        }

        public override State<BallViewStateMachine, BallViewEvent> goTo()
        {
            return new Uncompleting(myState.stateMachine);
        }
    }

    //internal class FinishedCompletingTransition : FinishedAnimationTransition
    //{
    //    public FinishedCompletingTransition(BallViewState state) : base(state)
    //    {
    //    }

    //    public override State<BallViewStateMachine, BallViewEvent> goTo()
    //    {
    //        return new Completed(myState.stateMachine);
    //    }
    //}
}

