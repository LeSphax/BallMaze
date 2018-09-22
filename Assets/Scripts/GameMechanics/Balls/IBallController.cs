using UnityEngine;

public interface IBallController
{

    event EmptyEventHandler FinishedAnimating;

    void Move(Direction direction);

    void MoveBack(int oldPosX, int oldPosY);
    bool IsEmpty();
    bool IsWall();

    void InitObjectiveType(ObjectiveType type);
    void Init(int x, int y, Board model);
    void Init(int x, int y, int z, CubeView model);


    Vector2 GetPosition();
    void SetPosition(Vector3 position);

    void Destroy();

    void FillObjective();
    void UnFillObjective();
    bool IsMoving();
    ObjectiveType GetObjectiveType();
    BallType GetBallType();
    void SetMesh(GameObject mesh);
}
