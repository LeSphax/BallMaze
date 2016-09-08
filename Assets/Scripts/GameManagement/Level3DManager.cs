using UnityEngine;

namespace BallMaze.GameManagement
{
    class Level3DManager : LevelManager
    {
        [SerializeField]
        

        [HideInInspector]

        internal override void LevelFinished()
        {
            Debug.Log("Need to implement LevelFinished");
        }

        internal override void NotifyFilledObjective(ObjectiveType objectiveEntered)
        {
            Debug.Log("Need to implement NotifyFilledObjective");
        }

        internal override void NotifyUnFilledObjective(ObjectiveType objectiveType)
        {
            Debug.Log("Need to implement NotifyUnFilledObjective");
        }

        internal override void SetObjectiveOrder(ObjectiveType firstObjective)
        {
            Debug.Log("Need to implement SetObjectiveOrder");
        }
    }
}
