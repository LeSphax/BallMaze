using UnityEngine;

public static class DebugExtensions
    {
    public static void Log(params object[] values)
    {
        string result = "";
        foreach(object value in values){
            result += value.ToString() + "   ";
        }
        Debug.Log(result);
    }
    }

