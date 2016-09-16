using BallMaze.GameMechanics.Tiles;
using CustomAnimations;
using CustomAnimations.BallMazeAnimations;
using UnityEngine;
using UnityEngine.Assertions;

namespace BallMaze.GameMechanics.ObjectiveBall
{

    public class ObjectiveBallView : BallView
    {

        public event EmptyEventHandler FinishedAnimating;

        private BallViewStateMachine stateMachine;
        private AnimationGroup completingAnimation;

        void Awake()
        {
            stateMachine = gameObject.AddComponent<BallViewStateMachine>();
        }

        public void SetTarget(Vector3 target)
        {
            Vector3 realTarget = target + Vector3.up * BALL_HALF_HEIGHT;
            stateMachine.handleEvent(new MoveCommand(realTarget));
        }

        internal void StartMovingTowards(Vector3 target)
        {
            Debug.Log("StartMovingTowards " + PlayBoard.TURN_DURATION);
            Movement2DAnimation currentAnimation = Movement2DAnimation.CreateMovement2DAnimation(gameObject, target, PlayBoard.TURN_DURATION);
            StartAnimation(currentAnimation);
        }

        private void StartAnimation(MyAnimation animation)
        {
            animation.FinishedAnimating += new AnimationEventHandler(AnimationFinished);
            animation.StartAnimating();
        }

        internal virtual void RaiseFinishedAnimating()
        {
           // Debug.Log(gameObject.name + "  RaiseFinished");
            FinishedAnimating.Invoke();
        }

        private void AnimationFinished(MonoBehaviour animation)
        {
            //Debug.Log(gameObject.name + "  AnimationFinished");
            stateMachine.handleEvent(new FinishedAnimation(animation));
        }

        public void CompleteView(TileController objective)
        {
            stateMachine.handleEvent(new CompleteCommand(objective));
        }

        internal void StartCompletingAnimation(TileController objective)
        {
            Vector3 target = objective.transform.localPosition - Vector3.up * 2;
            float duration = PlayBoard.TURN_DURATION * 3;
            completingAnimation = gameObject.AddComponent<AnimationGroup>();
            completingAnimation.AddAnimation(ColorAnimation.CreateColorAnimation(Mesh, Color.clear, duration));
            completingAnimation.AddAnimation(objective.GetFillingAnimation(duration));
            completingAnimation.AddAnimation(MovementAnimation.CreateMovementAnimation(gameObject, target, duration));
            completingAnimation.StartAnimating();
        }

        public void UnCompleteView(TileController objective)
        {
            stateMachine.handleEvent(new CompleteCommand(objective));
        }

        internal void StartUncompletingAnimation(TileController objective)
        {
            Assert.IsNotNull(completingAnimation);
            completingAnimation.FinishedAnimating += new AnimationEventHandler(AnimationFinished);
            completingAnimation.ChangeDuration(PlayBoard.TURN_DURATION * 3);
            completingAnimation.StartReverseAnimating();
        }

        internal void ActivateFloating(bool v)
        {
            if (GetComponent<FloatingAnimation>() != null)
                GetComponent<FloatingAnimation>().Activate(v);
        }
    }
}