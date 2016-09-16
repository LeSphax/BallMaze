using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Settings
{
    private const string path = "Settings";

    private static Dictionary<string, string> _data;
    private static Dictionary<string, string> data
    {
        get
        {
            if (_data == null)
            {
                TextAsset settings = (TextAsset)Resources.Load(path);
                _data = ParseSettings(settings.text);
            }
            return _data;
        }
    }

    public static string GetFirst2DLevelName()
    {
        return data["First2DLevelName"];
    }

    public static string GetFirst3DLevelName()
    {
        return data["First3DLevelName"];
    }

    private static Dictionary<string, string> ParseSettings(string settings)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();
        StringReader reader = new StringReader(settings);
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            int separatorIndex = line.IndexOf(':');
            string key = line.Substring(0, separatorIndex).Trim();
            string value = line.Substring(separatorIndex+1).Trim();
            result.Add(key, value);
        }
        return result;
    }






}
