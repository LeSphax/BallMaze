using UnityEngine;
using System.Collections.Generic;
using System;
using BallMaze.Inputs;
using Utilities;

namespace BallMaze.GameManagement
{
    public class SaveManager : MonoBehaviour
    {
        string path;
        string fileName = "myLogs.dat";

        public bool saveOnDisable = true;

        public void SetPlayerName()
        {
            //currentGameLogs.playerName = GameObject.FindGameObjectWithTag(Tags.PlayerNameInput).GetComponent<Text>().text;
            path = Application.persistentDataPath + "/" + currentGameLogs.playerName;
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
                GamesLogs _logs;
                Saving.TryLoad(path, out _logs);
                _logs.AddGame(currentGameLogs);
                Saving.Save(path, _logs);
            }
        }

        public static GamesLogs GetAllLogs(string fileName)
        {
            GamesLogs _logs;
            Saving.TryLoad(Application.persistentDataPath + "/" + fileName, out _logs);
            return _logs;
        }


        void OnDisable()
        {
            if (currentGameLogs != null && saveOnDisable)
            {
                currentGameLogs.dateSaved = DateTime.Today;
                Save();
            }
        }

        public string GetCurrentLevel()
        {
            GamesLogs logs;
            Saving.TryLoad(path,out logs);
            if (logs != null)
            {
                return logs.games[logs.games.Count - 1].lastLevelPlayed;
            }
            return "";
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

    [Serializable]
    public class GamesLogs
    {
        public List<GameLogs> games;

        public GamesLogs()
        {
            games = new List<GameLogs>();
        }

        public GamesLogs(List<GameLogs> games)
        {
            this.games = games;
        }

        public void AddGame(GameLogs game)
        {
            games.Add(game);
        }
    }
}
