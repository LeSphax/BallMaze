using UnityEngine;

namespace CustomAnimations.BallMazeAnimations
{
    public class Movement2DAnimation : MyAnimation
    {

        Vector3 startPoint;
        Vector3 targetPoint;

        public void Init(Vector3 targetPoint, float duration)
        {
            startPoint = gameObject.transform.localPosition;
            this.targetPoint = targetPoint;
            this.duration = duration;
        }

        public void UndoMovementAnimation()
        {
            StartReverseAnimating();
        }

        protected override void Animate(float completion)
        {

            Vector3 newVector = startPoint * (1 - completion) + targetPoint * completion;
            transform.localPosition = new Vector3(newVector.x, transform.localPosition.y, newVector.z);
        }

        public static Movement2DAnimation CreateMovement2DAnimation(GameObject animatedObject, Vector3 targetPoint, float duration)
        {
            Movement2DAnimation animation = animatedObject.AddComponent<Movement2DAnimation>();
            animation.Init(targetPoint, duration);
            return animation;
        }


    }
}