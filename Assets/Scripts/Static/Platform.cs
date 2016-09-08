public class Platform
{
    internal static bool Mobile()
    {
#if UNITY_WP_8_0 || UNITY_WP_8_1 || UNITY_ANDROID || UNITY_IPHONE
        return true;
#else
        return false;
#endif
    }
}
