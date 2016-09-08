
using GenericStateMachine;

namespace BallMaze.Cube
{
    internal class S_Camera : State<CameraStateMachine, E_Camera>
    {
        public CameraController CameraController
        {
            get
            {
                return stateMachine.cameraController;
            }
        }

        public S_Camera(CameraStateMachine stateMachine) : base(stateMachine)
        {
            new T_LevelChange(this);
        }
    }

    internal class S_Animation : S_Camera
    {

        public S_Animation(CameraStateMachine stateMachine) : base(stateMachine)
        {
            new T_DelayEvent(this);
        }
    }

    internal class S_Static : S_Camera
    {

        public S_Static(CameraStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void enter()
        {
            if (stateMachine.nextEvents.Count > 0)
            {
                stateMachine.handleEvent(stateMachine.nextEvents.Dequeue());
            }
        }
    }


    internal class S_Init : S_Camera
    {
        public override void enter()
        {
            handleEvent(new E_InitCamera());
        }

        public S_Init(CameraStateMachine stateMachine) : base(stateMachine)
        {
        }
    }

    internal class S_OnBoard : S_Static
    {
        public S_OnBoard(CameraStateMachine stateMachine) : base(stateMachine)
        {
            new T_SetPerspective(this);
            new T_IgnoreDirectionEvent(this);
        }
    }

    internal class S_OnCube : S_Static
    {
        public S_OnCube(CameraStateMachine stateMachine) : base(stateMachine)
        {
            new T_StartRotating(this);
            new T_SetOrtho(this);
        }
    }

    internal class S_FadingIn : S_Animation
    {
        public S_FadingIn(CameraStateMachine stateMachine) : base(stateMachine)
        {
            new T_FinishedFadingIn(this);
        }
    }

    internal class S_FadingOut : S_Animation
    {
        public S_FadingOut(CameraStateMachine stateMachine) : base(stateMachine)
        {
            new T_FinishedFadingOut(this);
        }
    }

    internal class S_Rotating : S_Animation
    {
        public S_Rotating(CameraStateMachine stateMachine) : base(stateMachine)
        {
            new T_FinishedRotating(this);
        }
    }
}