using UnityEngine;

namespace Navigation
{
    public class NavigationManager : MonoBehaviour
    {

        public static SceneLoader loader;
        private static NavigationMachine stateMachine;

        void Start()
        {
            loader = GetComponent<SceneLoader>();
            stateMachine = gameObject.AddComponent<NavigationMachine>();
        }

        public void Load2DGame()
        {
            stateMachine.handleEvent(new Load2DGameEvent());
        }

        public void LoadContinueScene()
        {
            Load2DGame();
        }

        public void Load3DGame()
        {
            stateMachine.handleEvent(new Load3DGameEvent());
        }

        public void GoBack()
        {
            stateMachine.handleEvent(new ReturnEvent());
        }

        internal static void LoadScene(string scene)
        {
            loader.FinishedLoading += FinishedLoading;
            loader.StartLoading(scene);
        }

        private static void FinishedLoading()
        {
            Debug.Log("Finished Loading");
            stateMachine.handleEvent(new FinishedLoadingScene());
            loader.FinishedLoading -= FinishedLoading;
        }
    }
}

