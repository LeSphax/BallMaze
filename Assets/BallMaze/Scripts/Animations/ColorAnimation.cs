using UnityEngine;
using UnityEngine.Assertions;

namespace CustomAnimations.BallMazeAnimations
{
    [RequireComponent(typeof(Material))]
    public class ColorAnimation : MyAnimation
    {

        Color[] startColors;
        Color endColor;
        Material[] myMaterials;

        public void Init(Color endColor, float duration, int materialNumber)
        {
            SetParameters(endColor, duration, materialNumber);
            startColors = new Color[myMaterials.Length];
            for (int i = 0; i < myMaterials.Length; i++)
            {
                startColors[i] = myMaterials[i].color;
            }
            
        }

        private void SetParameters(Color endColor, float duration, int materialNumber)
        {
            Renderer renderer = gameObject.GetComponent<Renderer>();
            if (materialNumber == -1)
            {
                myMaterials = renderer.materials;
            }
            else
            {
                Assert.IsTrue(materialNumber < renderer.materials.Length);
                myMaterials = new Material[1];
                myMaterials[0] = renderer.materials[materialNumber];
            }
            this.endColor = endColor;
            this.duration = duration;
        }

        public void Init(Color startColor, Color endColor, float duration, int materialNumber)
        {
            SetParameters(endColor, duration, materialNumber);
            startColors = new Color[1];
            startColors[0] = startColor;
            Assert.AreEqual(startColors.Length, myMaterials.Length);
        }

        protected override void Animate(float completion)
        {
            for (int i = 0; i < myMaterials.Length; i++)
            {
                myMaterials[i].color = startColors[i] * (1 - completion) + endColor * completion;
            }
        }

        public static ColorAnimation CreateColorAnimation(GameObject animatedObject, Color targetColor, float duration, int materialNumber = -1)
        {
            ColorAnimation animation = animatedObject.AddComponent<ColorAnimation>();
            animation.Init(targetColor, duration, materialNumber);
            return animation;
        }

        public static ColorAnimation CreateColorAnimation(GameObject animatedObject, Color startColor, Color targetColor, float duration, int materialNumber = -1)
        {
            ColorAnimation animation = animatedObject.AddComponent<ColorAnimation>();
            animation.Init(startColor, targetColor, duration, materialNumber);
            return animation;
        }


    }
}
