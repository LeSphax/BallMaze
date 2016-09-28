using UnityEngine;

namespace CustomAnimations.BallMazeAnimations
{
    public class RotateAroundAnimation : MyAnimation
    {
        public float inspectorDuration;
        public Transform center;
        float distanceToCenter;
        float startAngle;

        void Start()
        {
            if (inspectorDuration != 0)
            {
                Init();
                StartAnimating();
            }
        }

        public void Init()
        {

            InitParameters(center);
            duration = inspectorDuration;
        }

        public void Init(float duration, Transform center)
        {
            this.center = center;
            this.duration = duration;
            InitParameters(center);
        }

        private void InitParameters(Transform center)
        {
            Vector2 object2DPosition = new Vector2(transform.position.x, transform.position.z);
            Vector2 center2DPosition = new Vector2(center.position.x, center.position.z);
            distanceToCenter = Vector2.Distance(object2DPosition, center2DPosition);
            startAngle = Mathf.Atan2(transform.position.x - center.position.x, transform.position.z - center.position.z);
        }

        protected override void Animate(float completion)
        {
            float angle = startAngle + Mathf.Deg2Rad * 360 * completion;
            transform.localPosition = new Vector3(Mathf.Cos(angle) * distanceToCenter, transform.localPosition.y, Mathf.Sin(angle) * distanceToCenter);
        }

        protected override void FinishAnimation()
        {
            if (state == State.ANIMATING)
            {
                Animate(1);
                StartAnimating();
            }
        }

        public static RotateAroundAnimation AddRotateAroundAnimation(GameObject animatedObject, float duration, Transform center)
        {
            RotateAroundAnimation animation = animatedObject.AddComponent<RotateAroundAnimation>();
            animation.Init(duration, center);
            return animation;
        }

    }
}
