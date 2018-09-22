
using UnityEngine;

public abstract class LevelManager : MonoBehaviour
{

    internal abstract void NotifyFilledObjective(ObjectiveType objectiveEntered);

    internal abstract void NotifyUnFilledObjective(ObjectiveType objectiveType);

    internal abstract void SetObjectiveOrder(ObjectiveType firstObjective);

    internal abstract void LevelFinished();

}
