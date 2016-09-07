
using GenericStateMachine;
using UnityEngine.Assertions;

namespace BallMaze.Cube
{
    internal class T_Camera<EventType> : Transition<CameraStateMachine, E_Camera, EventType> where EventType : E_Camera
    {
        protected S_Camera state
        {
            get
            {
                Assert.IsTrue(typeof(S_Camera).IsAssignableFrom(myState.GetType()), "This state should be a CameraState " + myState.GetType());
                return (S_Camera)myState;
            }

            set
            {
                base.myState = value;
            }
        }

        public T_Camera(S_Camera myState) : base(myState)
        {
        }
    }

    internal class T_SetOrtho : T_Camera<E_ChangePerspective>
    {
        public T_SetOrtho(S_OnCube state) : base(state)
        {
        }

        public override void action(E_ChangePerspective evt)
        {
            state.CameraController.FadeIn();
        }

        public override State<CameraStateMachine, E_Camera> goTo()
        {
            return new S_FadingIn(myState.stateMachine);
        }
    }

    internal class T_FinishedFadingIn : T_Camera<E_FinishedAnimating>
    {
        public T_FinishedFadingIn(S_FadingIn state) : base(state)
        {
        }

        public override State<CameraStateMachine, E_Camera> goTo()
        {
            return new S_OnBoard(myState.stateMachine);
        }
    }

    internal class T_SetPerspective : T_Camera<E_ChangePerspective>
    {
        public T_SetPerspective(S_OnBoard state) : base(state)
        {
        }

        public override void action(E_ChangePerspective evt)
        {
            state.CameraController.FadeOut();
        }

        public override State<CameraStateMachine, E_Camera> goTo()
        {
            return new S_FadingOut(myState.stateMachine);
        }
    }

    internal class T_FinishedFadingOut : T_Camera<E_FinishedAnimating>
    {
        public T_FinishedFadingOut(S_FadingOut state) : base(state)
        {
        }

        public override State<CameraStateMachine, E_Camera> goTo()
        {
            return new S_OnCube(myState.stateMachine);
        }
    }

    internal class T_StartRotating : T_Camera<E_Direction>
    {
        public T_StartRotating(S_OnCube state) : base(state)
        {
        }

        public override State<CameraStateMachine, E_Camera> goTo()
        {
            return new S_Rotating(myState.stateMachine);
        }

        public override void action(E_Direction evt)
        {
            state.CameraController.TurnInDirection(evt.direction);
        }
    }

    internal class T_FinishedRotating : T_Camera<E_FinishedAnimating>
    {
        public T_FinishedRotating(S_Rotating state) : base(state)
        {
        }

        public override State<CameraStateMachine, E_Camera> goTo()
        {
            return new S_OnCube(myState.stateMachine);
        }
    }

    internal class T_IgnoreDirectionEvent : T_Camera<E_Direction>
    {
        public T_IgnoreDirectionEvent(S_OnBoard state) : base(state)
        {
        }

        public override State<CameraStateMachine, E_Camera> goTo()
        {
            return myState;
        }
    }

    internal class T_LevelChange : T_Camera<E_LevelChanged>
    {
        public T_LevelChange(S_Camera state) : base(state)
        {

        }

        public override State<CameraStateMachine, E_Camera> goTo()
        {
            return new S_Init(myState.stateMachine);
        }
    }

    internal class T_InitCamera : T_Camera<E_InitCamera>
    {
        public T_InitCamera(S_Camera state) : base(state)
        {

        }

        public override State<CameraStateMachine, E_Camera> goTo()
        {
            return new S_FadingIn(myState.stateMachine);
        }

        public override void action(E_InitCamera evt)
        {
            state.CameraController.Reset();
        }
    }

    internal class T_DelayEvent : T_Camera<E_Delayed>
    {
        public T_DelayEvent(S_Camera state) : base(state)
        {

        }

        public override State<CameraStateMachine, E_Camera> goTo()
        {
            return myState;
        }

        public override void action(E_Delayed evt)
        {
            state.stateMachine.nextEvents.Enqueue(evt);
        }
    }
}