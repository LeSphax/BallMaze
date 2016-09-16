using BallMaze;
using System;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;
using Utilities;

public abstract class LevelData
{


    public string previousLevelName;
    [XmlIgnore]
    private string fileName;
    public string FileName
    {
        get
        {
            return fileName;
        }
        set
        {
            fileName = value.Trim();
        }
    }
    public virtual string Name
    {
        get
        {
            string[] split = fileName.Split(Paths.FOLDER_SEPARATOR_CHAR);
            return split[split.Length-1];
        }
    }

    public string nextLevelName;
    protected abstract PuzzleData puzzleData
    {
        get;
        set;
    }
    public int numberMoves;

    private static LevelData _tempLevelData;
    public static LevelData tempLevelData
    {
        get
        {
            return _tempLevelData;
        }
        set
        {
            _tempLevelData = value;
        }
    }

    public ObjectiveType firstObjective;

    public LevelData()
    {

    }

    public LevelData(PuzzleData data, string name, string nextLevelName = "")
    {
        InitData(data, name, nextLevelName);
    }

    private void InitData(PuzzleData data, string name, string nextLevelName)
    {
        this.puzzleData = data;
        this.fileName = name;
        this.nextLevelName = nextLevelName;
    }

    public LevelData(PuzzleData data, string previousLevelName, string name, string nextLevelName)
    {
        this.previousLevelName = previousLevelName;
        InitData(data, name, nextLevelName);
    }

    public void SetFirstObjective(ObjectiveType first)
    {
        firstObjective = first;
    }

    public void SetNumberMoves(int numberMoves)
    {
        this.numberMoves = numberMoves;
    }


    public bool Save(string fileName, bool force = false)
    {
#if UNITY_WEBGL || UNITY_WEBPLAYER
            _tempLevelData = this;
            return true;
#endif
        return SaveToStreamingAssets(fileName, force);
    }

    private bool SaveToResources(string fileName, bool force)
    {
        string path = Application.dataPath + Paths.FOLDER_SEPARATOR + Paths.RESOURCES + Paths.LEVEL_FILES + fileName;
        return SaveFile(fileName, force, path);
    }

    private bool SaveFile(string fileName, bool force, string path)
    {
        if (!force && File.Exists(path))
        {
            return false;
        }
        Serialize(path);
        Debug.Log(fileName + " was successfully saved !");
        return true;
    }

    protected abstract void Serialize(string path);

    private bool SaveToStreamingAssets(string fileName, bool force)
    {
        string path = GetApplicationPath() + fileName;
        return SaveFile(fileName, force, path);
    }

    private static string GetApplicationPath()
    {
#if !UNITY_EDITOR
            return Paths.LEVEL_FILES;
#else
        return Application.streamingAssetsPath + Paths.FOLDER_SEPARATOR + Paths.LEVEL_FILES;
#endif

    }

    internal bool HasPreviousLevel()
    {
        return previousLevelName != "";
    }

    internal bool HasNextLevel()
    {
        return nextLevelName != "";
    }

    public static bool TryLoad<T>(string fileName, out T levelData) where T : LevelData
    {
#if !UNITY_EDITOR
            if (fileName == LevelCreatorController.TEMP_LEVEL_NAME)
            {
                levelData = _tempLevelData;
                return true;
            }
            else
            {
                return LoadFromResources(fileName, out levelData);
            }
#else
        return LoadFromStreamingAssets(fileName, out levelData);
#endif
    }

    private static bool LoadFromResources<T>(string fileName, out T levelData) where T : LevelData
    {
        string path = GetApplicationPath() + fileName;

        TextAsset textAsset = (TextAsset)Resources.Load(path);
        if (textAsset == null)
        {
            levelData = null;
            Debug.LogError("The file you are trying to load does not exist : (Path : " + path + " ) (FileName : " + fileName + " )");
            return false;
        }

        StringReader reader = new StringReader(textAsset.text);
        XmlSerializer xs = new XmlSerializer(typeof(T));
        levelData = (T)xs.Deserialize(reader);
        levelData.FileName = fileName;
        return true;
    }

    private static bool LoadFromStreamingAssets<T>(string fileName, out T levelData) where T : LevelData
    {
        string path = GetApplicationPath() + fileName;
        Debug.Log(typeof(T));
        bool successfullLoad = Saving.TryLoad<T>(path, out levelData);

        if (successfullLoad)
        {
            levelData.FileName = fileName;
            if (levelData.puzzleData.IsValid())
                return true;
            else
            {
                Debug.LogError("The boardData isn't valid ! The filename was " + fileName);
                return true;
            }
        }
        levelData = null;
        return false;
    }
}


