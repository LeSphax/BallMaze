using UnityEngine;

public class LogVisualiser : MonoBehaviour
{

    GamesLogs gamesLogs;
    GameLogs gameCurrent;
    Log logCurrent;
    int indexLogs;

    public string fileName;

    private enum State
    {
        IDLE,
        PLAYING,
    }
    private State state;

    void Start()
    {
        gamesLogs = SaveManager.GetAllLogs(fileName);
        gameCurrent = gamesLogs.games[0];
        indexLogs = 0;
        logCurrent = gameCurrent.logs[indexLogs];
        state = State.PLAYING;
    }

    void Update()
    {
        switch (state)
        {
            case State.PLAYING:
                float timeCurrent = Time.realtimeSinceStartup;
                if (timeCurrent > logCurrent.timeSaved)
                {
                    logCurrent.command.LogExecute();
                    if (indexLogs < gameCurrent.logs.Count - 1)
                    {
                        indexLogs++;
                        logCurrent = gameCurrent.logs[indexLogs];
                    }
                    else
                    {
                        state = State.IDLE;
                    }
                }
                break;
            case State.IDLE:
                break;
        }

    }
}
