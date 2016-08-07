
using UnityEngine;

namespace BallMaze.GameManagement
{
    public class NormalLevelManager : LevelManager
    {

        public ObjectiveType[] objectivesOrder;
        private int objectivesFilled = 0;

        private GameObject resetButton;

        void Awake()
        {
            resetButton = GameObject.FindGameObjectWithTag(Tags.ResetButton);
        }


        internal override void NotifyFilledObjective(ObjectiveType objectiveEntered)
        {
            if (objectivesFilled < objectivesOrder.Length && objectivesOrder[objectivesFilled] != objectiveEntered && objectivesOrder[objectivesFilled] != ObjectiveType.NONE)
            {
                Invoke("StartBlinking", 8.0f);
            }
            objectivesFilled++;
        }

        internal override void NotifyUnFilledObjective(ObjectiveType objectiveType)
        {
            objectivesFilled--;
            StopBlinking();
        }

        private void StopBlinking()
        {
            CancelInvoke();
            resetButton.SendMessage("StopBlinking");
        }

        internal override void SetObjectiveOrder(ObjectiveType firstObjective)
        {
            objectivesOrder = new ObjectiveType[1];
            objectivesOrder[0] = firstObjective;
        }

        void StartBlinking()
        {
            resetButton.SendMessage("Blink");
        }

        internal override void LevelFinished()
        {
            GameObject.FindGameObjectWithTag(Tags.BallMazeController).GetComponent<LevelLoader>().LoadNextLevel();
        }
    }
}