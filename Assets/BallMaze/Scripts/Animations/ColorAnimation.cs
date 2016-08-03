using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Material))]
public class ColorAnimation : MyAnimation
{

    Color startColor;
    Color endColor;
    Material myMaterial;

    public void Init(Color endColor, float duration)
    {
        SetParameters(endColor, duration);
        startColor = myMaterial.color;
    }

    private void SetParameters(Color endColor,float duration)
    {
       myMaterial =  gameObject.GetComponent<Renderer>().material;
        this.endColor = endColor;
        this.duration = duration;
    }

    public void Init(Color startColor, Color endColor, float duration)
    {
        SetParameters(endColor, duration);
        this.startColor = startColor;
       
    }

    protected override void Animate(float completion)
    {
        myMaterial.color = startColor * (1 - completion) + endColor * completion;
    }

    public static ColorAnimation CreateColorAnimation(GameObject animatedObject, Color targetColor, float duration)
    {
        ColorAnimation animation = animatedObject.AddComponent<ColorAnimation>();
        animation.Init(targetColor, duration);
        return animation;
    }

    public static ColorAnimation CreateColorAnimation(GameObject animatedObject, Color startColor, Color targetColor, float duration)
    {
        ColorAnimation animation = animatedObject.AddComponent<ColorAnimation>();
        animation.Init(startColor, targetColor, duration);
        return animation;
    }


}
