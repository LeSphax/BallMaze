using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Utilities;

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

    public static string GetFirstLevelName()
    {
        return data["FirstLevelName"];
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
