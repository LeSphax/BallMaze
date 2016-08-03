
using BallMaze.Exceptions;
using UnityEngine;

namespace BallMaze.GameMechanics
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


        internal static IBallModel NextBall(BallData ballData)
        {
            GameObject ball;
            IBallModel ballModel;
            switch (ballData.BallType)
            {
                case BallType.EMPTY:
                    ballModel = new EmptyBall();
                    break;
                case BallType.NORMAL:
                    switch (ballData.ObjectiveType)
                    {
                        case ObjectiveType.NONE:
                            ball = Object.Instantiate(NoObjectiveBallPrefab);
                            break;
                        case ObjectiveType.OBJECTIVE1:
                            ball = Object.Instantiate(Objective1BallPrefab);
                            break;
                        case ObjectiveType.OBJECTIVE2:
                            ball = Object.Instantiate(Objective2BallPrefab);
                            break;
                        default:
                            throw new UnhandledSwitchCaseException(ballData.ObjectiveType);
                    }
                    FloatingAnimation.AddFloatingAnimation(ball, 1, 0.4f);
                    ballModel = ball.AddComponent<ObjectiveBallModel>();
                    ball.AddComponent<ObjectiveBallView>();
                    break;
                case BallType.WALL:
                    ball = Object.Instantiate(WallPrefab);
                    ballModel = ball.AddComponent<BallModel>();
                    ball.AddComponent<BallView>();
                    break;
                default:
                    throw new UnhandledSwitchCaseException(ballData.BallType);

            }
            ballModel.InitObjectiveType(ballData.ObjectiveType);
            return ballModel;

        }
    }
}

