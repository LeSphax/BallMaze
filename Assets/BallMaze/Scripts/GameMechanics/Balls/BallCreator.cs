
using BallMaze.Exceptions;
using BallMaze.GameMechanics.ObjectiveBall;
using CustomAnimations.BallMazeAnimations;
using UnityEngine;
using UnityEngine.Assertions;

namespace BallMaze.GameMechanics.Balls
{
    class BallCreator
    {

        private static GameObject _WallPrefab;
        private static GameObject WallPrefab
        {
            get
            {
                if (_WallPrefab == null)
                {
                    _WallPrefab = Resources.Load<GameObject>(Paths.WALL);
                }
                return _WallPrefab;
            }
        }

        private static GameObject _NoObjectiveBallPrefab;
        private static GameObject NoObjectiveBallPrefab
        {
            get
            {
                if (_NoObjectiveBallPrefab == null)
                {
                    _NoObjectiveBallPrefab = Resources.Load<GameObject>(Paths.NO_OBJECTIVE_BALL);
                }
                return _NoObjectiveBallPrefab;
            }
        }

        private static GameObject _Objective1BallPrefab;
        private static GameObject Objective1BallPrefab
        {
            get
            {
                if (_Objective1BallPrefab == null)
                {
                    _Objective1BallPrefab = Resources.Load<GameObject>(Paths.OBJECTIVE1_BALL);
                }
                return _Objective1BallPrefab;
            }
        }

        private static GameObject _Objective2BallPrefab;
        private static GameObject Objective2BallPrefab
        {
            get
            {
                if (_Objective2BallPrefab == null)
                {
                    _Objective2BallPrefab = Resources.Load<GameObject>(Paths.OBJECTIVE2_BALL);
                }
                return _Objective2BallPrefab;
            }
        }

        internal static IBallController GetBall(BallData ballData, float sizeRatio, out GameObject gameObject)
        {
            GameObject mesh = null;
            IBallController ballController;
            switch (ballData.BallType)
            {
                case BallType.EMPTY:
                    gameObject = new GameObject("EmptyBall");
                    ballController = new EmptyBallController();
                    break;
                case BallType.NORMAL:
                case BallType.WALL:
                    gameObject = new GameObject();
                    if (ballData.BallType == BallType.NORMAL)
                    {
                        switch (ballData.ObjectiveType)
                        {
                            case ObjectiveType.NONE:
                                mesh = Object.Instantiate(NoObjectiveBallPrefab);
                                break;
                            case ObjectiveType.OBJECTIVE1:
                                mesh = Object.Instantiate(Objective1BallPrefab);
                                break;
                            case ObjectiveType.OBJECTIVE2:
                                mesh = Object.Instantiate(Objective2BallPrefab);
                                break;
                            default:
                                throw new UnhandledSwitchCaseException(ballData.ObjectiveType);
                        }
                        FloatingAnimation.AddFloatingAnimation(mesh, 1, 0.4f);
                        ballController = gameObject.AddComponent<ObjectiveBallController>();
                    }
                    else
                    {
                        mesh = Object.Instantiate(WallPrefab);
                        ballController = gameObject.AddComponent<WallController>();
                    }
                    Assert.IsNotNull(mesh);
                    ballController.SetMesh(mesh);

                    break;
                default:
                    throw new UnhandledSwitchCaseException(ballData.BallType);

            }
            if (mesh != null)
                mesh.transform.localScale *= sizeRatio;
            ballController.InitObjectiveType(ballData.ObjectiveType);
            return ballController;
        }

        internal static IBallController GetBall(BallData ballData, float sizeRatio)
        {
            GameObject trash;
            return GetBall(ballData, sizeRatio, out trash);
        }
    }
}

