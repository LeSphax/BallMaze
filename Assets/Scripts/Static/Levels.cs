
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

    public static bool is3DLevel(string fileName)
    {
        Debug.Log(fileName);
        if (fileName.Substring(0, Paths.DIR_2D.Length) == Paths.DIR_2D)
            return false;
        else return true;
    }

    public static string GetFirst2DLevelName()
    {
        return Paths.DIR_2D + levels[Paths.DIR_2D.Substring(0,Paths.DIR_2D.Length-1)].First.Value;
    }

    public static string GetFirst3DLevelName()
    {
        return Paths.DIR_3D + levels[Paths.DIR_3D.Substring(0, Paths.DIR_2D.Length - 1)].First.Value;
    }

    public static string GetNextLevelName(string currentLevel)
    {
        string directoryName;
        string levelName;
        bool levelOk = CheckLevel(currentLevel, out directoryName, out levelName);
        if (levelOk && levels[directoryName].Find(levelName).Next != null)
            return directoryName + "/" + levels[directoryName].Find(levelName).Next.Value;
        return null;
    }

    public static string GetPreviousLevelName(string currentLevel)
    {
        string directoryName;
        string levelName;
        bool levelOk = CheckLevel(currentLevel, out directoryName, out levelName);
        if (levelOk && levels[directoryName].Find(levelName).Previous != null)
            return directoryName + "/" + levels[directoryName].Find(levelName).Previous.Value;
        return null;
    }

    private static bool CheckLevel(string currentLevel, out string directoryName, out string levelName)
    {
        string[] split = currentLevel.Split(Paths.FOLDER_SEPARATOR_CHAR);
        directoryName = split[0];
        levelName = split[1];
        if (!levels.ContainsKey(directoryName))
        {
            Debug.LogError("This directory wasn't registered '" + directoryName+"' " + levels.Count);
            return false;
        }
        else if (levels[directoryName].Find(levelName) == null)
        {
            Debug.LogError("The current level doesn't exist " + levelName + "   " + levels[directoryName].Count);
            return false;
        }
        return true;
    }

    private static string GetDirectoryName(string currentLevel)
    {
        return currentLevel.Split(Paths.FOLDER_SEPARATOR_CHAR)[0];
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

#if UNITY_EDITOR && !UNITY_WEBPLAYER
    public static void WriteLevels()
    {
        using (StreamWriter outputFile = System.IO.File.AppendText("Assets/Resources/" + levelFileName + ".txt"))
        {
            string[] directories = Directory.GetDirectories(StreamingAssetsPath);
            foreach (string directoryPath in directories)
            {
                string directoryName = directoryPath.Substring(StreamingAssetsPath.Length + 1);
                if (!levels.ContainsKey(directoryName))
                {
                    levels.Add(directoryName, new LinkedList<string>());
                    outputFile.WriteLine("_" + directoryName);
                }
                string[] fileNames = Directory.GetFiles(directoryPath);
                //
                foreach (string fileName in fileNames)
                {
                    //Debug.Log(fileName);
                    if (fileName.EndsWith(".meta"))
                    {
                        continue;
                    }
                    string levelName = fileName.Substring(directoryPath.Length + 1);
                    levelName = Path.GetFileNameWithoutExtension(levelName);
                    if (!levels[directoryName].Contains(levelName))
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
