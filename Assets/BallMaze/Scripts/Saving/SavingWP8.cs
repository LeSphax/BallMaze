using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace BallMaze.Saving
{
    public static class SavingWP8
    {

        public static GamesLogs GetGamesLogs(string path)
        {
            return Load(path);
        }

        public static void Save(string path, SaveManager.GameLogs gameLogs)
        {
            GamesLogs _logs = Load(path);
            _logs.AddGame(gameLogs);
            FileStream file = File.Create(path);


            XmlSerializer xs = new XmlSerializer(typeof(GamesLogs));
            xs.Serialize(file, _logs);
            file.Close();
        }

        private static GamesLogs Load(string path)
        {

            if (File.Exists(path))
            {
                FileStream file = File.Open(path, FileMode.Open);

                XmlSerializer xs = new XmlSerializer(typeof(GamesLogs));
                GamesLogs logs = (GamesLogs)xs.Deserialize(file);
                file.Close();
                return logs;
            }
            return new GamesLogs();
        }

        [Serializable]
        public class GamesLogs
        {
            public List<SaveManager.GameLogs> games;

            public GamesLogs()
            {
                games = new List<SaveManager.GameLogs>();
            }

            public GamesLogs(List<SaveManager.GameLogs> games)
            {
                this.games = games;
            }

            public void AddGame(SaveManager.GameLogs game)
            {
                games.Add(game);
            }
        }
    }
}
