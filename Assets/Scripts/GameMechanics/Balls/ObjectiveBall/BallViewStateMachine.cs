using GenericStateMachine;

namespace BVStateMachine
{
    internal class BallViewStateMachine : StateMachine<BallViewStateMachine, BallViewEvent>
    {
        internal ObjectiveBallView view;

        protected override void Awake()
        {
            base.Awake();
            view = GetComponent<ObjectiveBallView>();
        }

        internal override State<BallViewStateMachine, BallViewEvent> DefineFirst()
        {
            return new Idle(this);
        }
    }

    internal abstract class BallViewState : State<BallViewStateMachine, BallViewEvent>
    {
        public BallViewState(BallViewStateMachine stateMachine) : base(stateMachine)
        {
        }
    }


    internal class StaticState : BallViewState
    {
        public StaticState(BallViewStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void enter()
        {
            stateMachine.view.RaiseFinishedAnimating();
        }
    }

    internal class Idle : StaticState
    {
        public Idle(BallViewStateMachine stateMachine) : base(stateMachine)
        {
            new MoveTransition(this);
            new CompletingTransition(this);
        }

        public override void enter()
        {
            base.enter();
            stateMachine.view.ActivateFloating(true);
        }
    }

    internal class Moving : BallViewState
    {
        public Moving(BallViewStateMachine stateMachine) : base(stateMachine)
        {
            new ReturnToIdleTransition(this);
        }

        public override void enter()
        {
            stateMachine.view.ActivateFloating(true);
        }
    }

    internal class Completed : StaticState
    {
        public Completed(BallViewStateMachine stateMachine) : base(stateMachine)
        {
            new UnCompletingTransition(this);
        }

        public override void enter()
        {
            base.enter();
            stateMachine.view.ActivateFloating(false);
        }
    }

    internal class Uncompleting : BallViewState
    {

        public Uncompleting(BallViewStateMachine stateMachine) : base(stateMachine)
        {
            new ReturnToIdleTransition(this);
        }

        public override void enter()
        {
            stateMachine.view.ActivateFloating(false);
        }
    }
}

