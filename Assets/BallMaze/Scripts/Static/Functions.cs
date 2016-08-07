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

}