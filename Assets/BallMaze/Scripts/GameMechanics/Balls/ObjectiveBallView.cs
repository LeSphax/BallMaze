using BallMaze.GameMechanics.Tiles;
using UnityEngine;
namespace BallMaze.GameMechanics
{

    internal class ObjectiveBallView : BallView
    {

        public event EmptyEventHandler FinishedAnimating;

        private TileModel objectiveTile;

        internal enum State
        {
            IDLE,
            MOVING,
            COMPLETING,
            UNCOMPLETING,
            LEAVING,
            FLOATING_AROUND,
            COMING_BACK,
        }

        private State state;

        public Color Color
        {
            get
            {
                return GetComponent<Renderer>().material.color;
            }
            internal set
            {
                GetComponent<Renderer>().material.color = value;
            }
        }

        void Start()
        {
            SetState(State.IDLE);
        }

        public void SetTarget(Vector3 target)
        {
            if (state == State.IDLE)
            {
                Vector3 realTarget = target + Vector3.up * BALL_HALF_HEIGHT;
                StartMovingTowards(realTarget);
            }
            else
            {
                Debug.LogError(state + ": Need to wait for movement to finish before starting another one");
            }
        }

        private void StartMovingTowards(Vector3 target)
        {
            SetState(State.MOVING);
            Movement2DAnimation currentAnimation = Movement2DAnimation.CreateMovement2DAnimation(gameObject, target, PlayBoardModel.TURN_DURATION);
            StartAnimation(currentAnimation);
        }

        private void StartAnimation(MyAnimation animation)
        {
            animation.FinishedAnimating += new AnimationEventHandler(AnimationFinished);
            animation.StartAnimating();
        }

        protected virtual void RaiseFinishedAnimating()
        {
            FinishedAnimating.Invoke();
        }

        private void AnimationFinished(MonoBehaviour animation)
        {
            switch (state)
            {
                case State.IDLE:
                    break;
                case State.MOVING:
                    SetState(State.IDLE);
                    RaiseFinishedAnimating();
                    Destroy(animation);
                    break;
                case State.COMPLETING:
                    StartLeavingAnimation();
                    RaiseFinishedAnimating();
                    Destroy(animation);
                    break;
                case State.UNCOMPLETING:
                    SetState(State.IDLE);
                    RaiseFinishedAnimating();
                    Destroy(animation);
                    break;
                case State.LEAVING:
                    SetState(State.FLOATING_AROUND);
                    break;
                case State.COMING_BACK:
                    StartUncompletingAnimation();
                    Destroy(animation);
                    break;
                case State.FLOATING_AROUND:
                    break;
            }
        }

        private void SetState(State newState)
        {
            switch (newState)
            {
                case State.IDLE:
                    ActivateFloating(true);
                    // GetComponent<Renderer>().enabled = true;
                    break;
                case State.MOVING:
                    ActivateFloating(true);
                    //GetComponent<Renderer>().enabled = true;
                    break;
                case State.COMPLETING:
                    ActivateFloating(false);
                    break;
                case State.UNCOMPLETING:
                    ActivateFloating(false);
                    break;
                case State.LEAVING:
                    ActivateFloating(false);
                    break;
                case State.COMING_BACK:
                    ActivateFloating(false);
                    break;
                case State.FLOATING_AROUND:
                    ActivateFloating(true);
                    //RotateAroundAnimation.AddRotateAroundAnimation(gameObject, 10, GameObject.FindGameObjectWithTag(Tags.Piedestal).transform).StartAnimating();
                    break;
                default:
                    break;
            }
            this.state = newState;
        }

        private void StartLeavingAnimation()
        {
            SetState(State.LEAVING);
            //if (GameObject.FindGameObjectWithTag(Subject4087.Tags.World) != null)
            //{
            //    transform.SetParent(GameObject.FindGameObjectWithTag(Subject4087.Tags.World).transform);
            //}
            Vector3 target = GetRandomPositionInDome();
            MovementAnimation moveAnimation = MovementAnimation.CreateMovementAnimation(gameObject, target, 1);
            StartAnimation(moveAnimation);
        }

        public void FillAnimation(TileModel objective)
        {
            SetState(State.COMPLETING);
            Vector3 target = transform.localPosition - Vector3.up * 2;
            AnimationGroup animationGroup = gameObject.AddComponent<AnimationGroup>();
            animationGroup.AddAnimation(ColorAnimation.CreateColorAnimation(objective.gameObject, Color.green, PlayBoardModel.TURN_DURATION));
            animationGroup.AddAnimation(MovementAnimation.CreateMovementAnimation(gameObject, target, PlayBoardModel.TURN_DURATION));
            animationGroup.FinishedAnimating += new AnimationEventHandler(AnimationFinished);
            animationGroup.StartAnimating();
        }

        public void ComingBackAnimation(TileModel objective)
        {
            SetState(State.COMING_BACK);
            objectiveTile = objective;
            GetComponent<MovementAnimation>().SetDuration(PlayBoardModel.TURN_DURATION);
            GetComponent<MovementAnimation>().UndoMovementAnimation();
        }

        private void StartUncompletingAnimation()
        {
            SetState(State.UNCOMPLETING);
            transform.SetParent(GameObject.FindGameObjectWithTag(Tags.LevelController).transform);
            Vector3 target = transform.localPosition + Vector3.up * 2;
            AnimationGroup animationGroup = gameObject.AddComponent<AnimationGroup>();
            animationGroup.AddAnimation(ColorAnimation.CreateColorAnimation(objectiveTile.gameObject, Color, PlayBoardModel.TURN_DURATION / 2));
            animationGroup.AddAnimation(MovementAnimation.CreateMovementAnimation(gameObject, target, PlayBoardModel.TURN_DURATION / 2));
            animationGroup.FinishedAnimating += new AnimationEventHandler(AnimationFinished);
            animationGroup.StartAnimating();
        }




        public Vector3 GetRandomPositionInDome()
        {
            float SIZE_DOME = 6f;

            float x, y, z;
            x = Random.Range(-SIZE_DOME, SIZE_DOME);
            y = Random.Range(-1, -SIZE_DOME + 2);
            z = Random.Range(-SIZE_DOME, SIZE_DOME);
            while (x * x + y * y + z * z > SIZE_DOME * SIZE_DOME)
            {
                x = Random.Range(-SIZE_DOME, SIZE_DOME);
                y = Random.Range(-1, -SIZE_DOME + 2);
                z = Random.Range(-SIZE_DOME, SIZE_DOME);
            }
            return new Vector3(x, y, z);
        }

        private void ActivateFloating(bool v)
        {
            if (GetComponent<FloatingAnimation>() != null)
                GetComponent<FloatingAnimation>().Activate(v);
        }
    }
}