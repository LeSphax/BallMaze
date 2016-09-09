using UnityEngine;

public static class Functions
{

    public static bool RaycastMouse(out RaycastHit hit)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            return true;
        return false;
    }

    public static float mod(this float x, float m)
    {
        return (x % m + m) % m;
    }

    public static int mod(this int x, int m)
    {
        return (x % m + m) % m;
    }



    public static int Inverse(int value, int size, bool inverse)
    {
        if (inverse)
        {
            value = size - 1 - value;
        }
        return value;
    }

}