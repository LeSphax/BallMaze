namespace Navigation
{

    public abstract class NavigationEvents{ }

    internal class LoadSceneEvent : NavigationEvents { }

    internal class Load2DGameEvent : LoadSceneEvent{ }

    internal class Load3DGameEvent : LoadSceneEvent{ }
    internal class ReturnEvent : LoadSceneEvent{ }
    internal class FinishedLoadingScene : NavigationEvents { }
}
