using UnityEngine;
using UnityEngine.UI;

public class BlinkingButton : MonoBehaviour
{
    private bool blinking = false;
    private float lastBlinkTime = 0;
    private float blinkInterval = 0.5f;

    private Image myRenderer;

    private Color colorOn = Color.white;
    private Color colorOff = Color.grey;


    void Awake()
    {
        myRenderer = GetComponent<Image>();
        StopBlinking();
    }

    public void Blink()
    {
        blinking = true;
    }

    public void StopBlinking()
    {
        myRenderer.color = colorOn;
        blinking = false;
    }

    void Update()
    {
        if (blinking)
        {
            if (Time.realtimeSinceStartup - lastBlinkTime > blinkInterval)
            {
                lastBlinkTime = Time.realtimeSinceStartup;
                if (myRenderer.color == colorOn)
                    SetSpriteOff();
                else
                    SetSpriteOn();
            }
        }
    }

    public void BlinkOnce()
    {
        SetSpriteOff();
        Invoke("SetSpriteOn", blinkInterval);
    }

    private void SetSpriteOn()
    {
        myRenderer.color = colorOn;
    }

    private void SetSpriteOff()
    {
        myRenderer.color = colorOff;
    }

}
