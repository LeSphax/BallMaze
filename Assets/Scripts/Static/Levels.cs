using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Levels
{
    public const string levelFileName = "Levels";

    public static string ResourcesPath
    {
        get
        {
            return "Assets/Resources/LevelFiles";
        }
    }
    public static string StreamingAssetsPath
    {
        get
        {
            return "Assets/StreamingAssets/LevelFiles";
        }
    }

    private static Dictionary<string, LinkedList<string>> _levels;
    private static Dictionary<string, LinkedList<string>> levels
    {
        get
        {
            if (_levels == null)
            {
                TextAsset levelFile = (TextAsset)Resources.Load(levelFileName);
                if (levelFile != null)
                    _levels = ParseLevels(levelFile.text);
                else
                    _levels = new Dictionary<string, LinkedList<string>>();
            }
            return _levels;
        }
    }

    public static string GetFirst2DLevelName()
    {
        return levels[BoardLevelData.DirectoryName()].First.Value;
    }

    public static string GetFirst3DLevelName()
    {
        return levels[CubeLevelData.DirectoryName()].First.Value;
    }

    public static string GetNextLevelName(PuzzleType type, string currentLevel)
    {
        string directoryName = PuzzleData.GetDirectoryName(type);
        if (levels.ContainsKey(directoryName) && levels[directoryName].Find(currentLevel).Next != null)
            return levels[directoryName].Find(currentLevel).Next.Value;
        return "";
    }

    public static string GetPreviousLevelName(PuzzleType type, string currentLevel)
    {
        string directoryName = PuzzleData.GetDirectoryName(type);
        if (levels.ContainsKey(directoryName) && levels[directoryName].Find(currentLevel).Previous != null)
            return levels[directoryName].Find(currentLevel).Previous.Value;
        return "";
    }

    private static Dictionary<string, LinkedList<string>> ParseLevels(string levels)
    {
        Dictionary<string, LinkedList<string>> result = new Dictionary<string, LinkedList<string>>();
        StringReader reader = new StringReader(levels);
        string line;
        string currentFolder = "Root";
        while ((line = reader.ReadLine()) != null)
        {
            if (line[0] == '_')
            {
                currentFolder = line.Substring(1);
            }
            else
            {
                if (!result.ContainsKey(currentFolder))
                {
                    result.Add(currentFolder, new LinkedList<string>());
                }
                result[currentFolder].AddLast(line);
            }
        }
        return result;
    }

#if UNITY_EDITOR
    public static void WriteLevels()
    {
        using (StreamWriter outputFile = File.AppendText("Assets/Resources/" + levelFileName + ".txt"))
        {
            string[] directories = Directory.GetDirectories(StreamingAssetsPath);
            foreach (string directoryName in directories)
            {
                outputFile.WriteLine("_"+directoryName);
                string directoryPath = StreamingAssetsPath + "/" + directoryName;
                string[] fileNames = Directory.GetFiles(directoryPath);
                Debug.Log(StreamingAssetsPath + "/" + directoryName);
                //
                foreach (string fileName in fileNames)
                {
                    Debug.Log(fileName);
                    Debug.Log(ResourcesPath);
                    if (fileName.EndsWith(".meta"))
                    {
                        continue;
                    }
                    string levelName = fileName.Substring(directoryPath.Length + 1);
                    levelName = Path.GetFileNameWithoutExtension(levelName);
                    if (!levels.ContainsKey(directoryName) || !levels[directoryName].Contains(levelName))
                    {
                        levels[directoryName].AddLast(levelName);
                        outputFile.WriteLine(levelName);
                    }
                }
            }
        }
    }
#endif






}
