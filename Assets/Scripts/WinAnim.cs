using CustomAnimations;
using UnityEngine;

public class WinAnim : MyAnimation
{
    private Vector3 sizeMesh;
    private Vector3 targetScale;

    protected override void Animate(float completion)
    {
        GetComponent<Renderer>().materials[0].color = GetComponent<Renderer>().materials[0].color * new Vector4(1, 1, 1, 1 - completion);
        transform.localScale = transform.localScale * (1 - completion) + completion * targetScale;
        Vector3 newRotation = transform.localRotation.eulerAngles.Multiply(new Vector3(1,1, 0));
        newRotation.z = 1800 * completion;
        transform.localRotation= Quaternion.Euler(newRotation.x,newRotation.y,newRotation.z);
    }

    // Use this for initialization
    void Start()
    {
        duration = 1.0f;
        targetScale = transform.localScale * 4;
        Vector3 target = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2 , Screen.height / 2, 10));
        MovementAnimation currentAnimation = MovementAnimation.CreateMovementAnimation(gameObject, target, duration);
        currentAnimation.StartAnimating();
        StartAnimating();
        //transform.LookAt(Camera.main.transform);
    }
}
