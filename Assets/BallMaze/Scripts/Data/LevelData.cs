using BallMaze.LevelCreation;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

namespace BallMaze.Data
{
    public class LevelData
    {
        public string previousLevelName;
        [XmlIgnore]
        public string name;
        public string nextLevelName;
        public BoardData boardData;
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

        public LevelData(BoardData data, string name, string nextLevelName = "")
        {
            InitData(data, name, nextLevelName);
        }

        private void InitData(BoardData data, string name, string nextLevelName)
        {
            this.boardData = data;
            this.name = name;
            this.nextLevelName = nextLevelName;
        }

        public LevelData(BoardData data, string previousLevelName, string name, string nextLevelName)
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
#if UNITY_EDITOR
            SaveToResources(fileName, force);

#elif UNITY_WEBGL || UNITY_WEBPLAYER
            _tempLevelData = this;
            return true;
#endif
            return SaveToStreamingAssets(fileName, force);
        }

        private bool SaveToResources(string fileName, bool force)
        {
            string path = Application.dataPath + Paths.FOLDER_SEPARATOR + Paths.RESOURCES + Paths.LEVEL_FILES + fileName + ".xml";
            return SaveFile(fileName, force, path);
        }

        private bool SaveFile(string fileName, bool force, string path)
        {
            if (!force && File.Exists(path))
            {
                return false;
            }
            FileStream file = File.Create(path);

            XmlSerializer xs = new XmlSerializer(typeof(LevelData));
            xs.Serialize(file, this);
            file.Close();
            Debug.Log(fileName + " was successfully saved !");
            return true;
        }

        private bool SaveToStreamingAssets(string fileName, bool force)
        {
            string path = GetApplicationPath() + fileName + ".xml";
            return SaveFile(fileName, force, path);
        }

        private static string GetApplicationPath()
        {
#if (UNITY_WEBGL || UNITY_WEBPLAYER) && ! UNITY_EDITOR
            return Paths.LEVEL_FILES;
#else
            return Application.streamingAssetsPath +  Paths.FOLDER_SEPARATOR + Paths.LEVEL_FILES;
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

        public static bool TryLoad(string fileName, out LevelData levelData)
        {
#if (UNITY_WEBGL || UNITY_WEBPLAYER) && ! UNITY_EDITOR
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

        private static bool LoadFromResources(string fileName, out LevelData levelData)
        {
            string path = GetApplicationPath() + fileName;
            TextAsset textAsset = (TextAsset)Resources.Load(path);
            if (textAsset == null ){
                levelData = null;
                Debug.LogError("The file you are trying to load does not exist : (Path : " + path + " ) (FileName : " + fileName + " )");
                return false;
            }

            StringReader reader = new StringReader(textAsset.text);
            XmlSerializer xs = new XmlSerializer(typeof(LevelData));
            levelData = (LevelData)xs.Deserialize(reader);
            levelData.name = fileName;
            return true;
        }

        private static bool LoadFromStreamingAssets(string fileName, out LevelData levelData)
        {
            string path = GetApplicationPath() + fileName + ".xml";
            if (File.Exists(path))
            {
                FileStream file = File.Open(path, FileMode.Open);

                XmlSerializer xs = new XmlSerializer(typeof(LevelData));
                levelData = (LevelData)xs.Deserialize(file);
                levelData.name = fileName;
                file.Close();
                if (levelData.boardData.IsValid())
                    return true;
                else
                {
                    Debug.LogError("The boardData isn't valid ! The filename was " + fileName);
                    return true;
                }
            }
            else
            {
                Debug.LogError("The file you are trying to load does not exist : (Path : " + path + " ) (FileName : "+ fileName +" )");
                levelData = null;
                return false;
            }
        }
    }
}

