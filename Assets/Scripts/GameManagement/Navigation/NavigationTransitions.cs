using GenericStateMachine;
using System;

namespace Navigation
{

    internal class LoadingSceneTransition<EventT> : Transition<NavigationMachine, NavigationEvents, EventT>  where EventT : LoadSceneEvent
    {
        string sceneName;
        public LoadingSceneTransition(State<NavigationMachine, NavigationEvents> myState, string sceneName) : base(myState)
        {
            this.sceneName = sceneName;
        }

        public override void action(EventT evt)
        {
            NavigationManager.LoadScene(sceneName);
        }

        public override State<NavigationMachine, NavigationEvents> goTo()
        {
            return new LoadingSceneState(myState.stateMachine);
        }

    }


    internal class ReturnTransition : LoadingSceneTransition<ReturnEvent>
    {
        public ReturnTransition(NavigationState myState) : base(myState,Scenes.MainMenu)
        {
        }        
    }

    internal class Load2DGameTransition : LoadingSceneTransition<Load2DGameEvent>
    {
        public Load2DGameTransition(NavigationState myState) : base(myState,Scenes.Game2D)
        {
        }

    }

    internal class Load3DGameTransition : LoadingSceneTransition<Load3DGameEvent>
    {
        public Load3DGameTransition(State<NavigationMachine, NavigationEvents> myState) : base(myState,Scenes.Game3D)
        {
        }
    }

    internal class EmptyLoadSceneTransition : Transition<NavigationMachine, NavigationEvents, LoadSceneEvent>
    {
        public EmptyLoadSceneTransition(State<NavigationMachine, NavigationEvents> myState) : base(myState)
        {
        }
    }

    internal class FinishedLoadingTransition : Transition<NavigationMachine, NavigationEvents, FinishedLoadingScene>
    {
        internal FinishedLoadingTransition(State<NavigationMachine, NavigationEvents> myState) : base(myState)
        {
        }

        public override State<NavigationMachine, NavigationEvents> goTo()
        {
            return myState.stateMachine.GetStateFromCurrentScene();
        }
    }
}
