using UnityEngine;
using System.Collections.Generic;
using System;
using BallMaze.Inputs;

namespace BallMaze.Saving
{
    public class SaveManager : MonoBehaviour
    {
        string path;
        string fileName = "myLogs.dat";

        public bool saveOnDisable = true;

        public void SetPlayerName()
        {
            //currentGameLogs.playerName = GameObject.FindGameObjectWithTag(Tags.PlayerNameInput).GetComponent<Text>().text;
            path = Application.persistentDataPath + "/" + currentGameLogs.playerName + ".xml";
        }


        private GameLogs currentGameLogs;

        void Awake()
        {
            path = Application.persistentDataPath + "/" + fileName;
        }

        // Use this for initialization
        void Start()
        {
            currentGameLogs = new GameLogs();
        }

        public void AddLog(InputCommand command)
        {
            currentGameLogs.AddLog(new Log(command, Time.realtimeSinceStartup));
        }

        public void Save()
        {
            if (path != null)
            {
                SavingWP8.Save(path, currentGameLogs);
            }
        }

        public static SavingWP8.GamesLogs GetAllLogs(string fileName)
        {
            return SavingWP8.GetGamesLogs(Application.persistentDataPath + "/" + fileName);
        }


        void OnDisable()
        {
            if (currentGameLogs != null && saveOnDisable)
            {
                currentGameLogs.dateSaved = DateTime.Today;
                Save();
            }
        }

        public class Log
        {
            public InputCommand command;
            public float timeSaved;

            public Log()
            {

            }

            public Log(InputCommand command, float timeSaved)
            {
                this.command = command;
                this.timeSaved = timeSaved;
            }
        }

        public string GetCurrentLevel()
        {
            SavingWP8.GamesLogs logs = SavingWP8.GetGamesLogs(path);
            if (SavingWP8.GetGamesLogs(path) != null)
            {
                return logs.games[logs.games.Count - 1].lastLevelPlayed;
            }
            return "";
        }



        [Serializable]
        public class GameLogs
        {
            public List<Log> logs;
            public string playerName;
            public DateTime dateSaved;
            public string lastLevelPlayed;

            public GameLogs()
            {
                logs = new List<Log>();
            }

            public void AddLog(Log log)
            {
                logs.Add(log);
            }

            public void print()
            {
                foreach (Log log in logs)
                {
                    Debug.Log(log.command + "   " + log.timeSaved);
                }
            }
        }
    }

}