using GenericStateMachine;
using System;
using UnityEngine.SceneManagement;

namespace Navigation
{
    internal class NavigationMachine : StateMachine<NavigationMachine,NavigationEvents>
    {

        internal override State<NavigationMachine, NavigationEvents> DefineFirst()
        {
            return GetStateFromCurrentScene();
        }

        public State<NavigationMachine, NavigationEvents> GetStateFromCurrentScene()
        {
            string nameCurrentScene = SceneManager.GetActiveScene().name;
            if (nameCurrentScene == Scenes.MainMenu)
            {
                return new MainMenuState(this);
            }
            else if (nameCurrentScene == Scenes.Game2D)
            {
                return new Game2DState(this);
            }
            else if (nameCurrentScene == Scenes.Game3D)
            {
                return new Game3DState(this);
            }
            else
            {
                throw new Exception("This Scene haven't been registered in the Navigation Manager : " + nameCurrentScene);
            }
        }
    }

    internal class NavigationState : State<NavigationMachine, NavigationEvents>
    {
        public NavigationState(NavigationMachine stateMachine) : base(stateMachine)
        {

        }
    }

    internal class MainMenuState : NavigationState
    {
        public MainMenuState(NavigationMachine stateMachine) : base(stateMachine)
        {
            new Load3DGameTransition(this);
            new Load2DGameTransition(this);
        }

    }

    internal class Game3DState : NavigationState
    {
        public Game3DState(NavigationMachine stateMachine) : base(stateMachine)
        {
            new ReturnTransition(this);
        }
    }

    internal class Game2DState : NavigationState
    {
        public Game2DState(NavigationMachine stateMachine) : base(stateMachine)
        {
            new ReturnTransition(this);
        }
    }

    internal class LoadingSceneState : NavigationState
    {
        public LoadingSceneState(NavigationMachine stateMachine) : base(stateMachine)
        {
            new EmptyLoadSceneTransition(this);
            new FinishedLoadingTransition(this);
        }
    }
}
