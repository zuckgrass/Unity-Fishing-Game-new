using UnityEngine;
using UnityEngine.UI;

public class BlinkingButton : MonoBehaviour
{
    public Button button;                  // Reference to the button
    public Color startColor = Color.red;   // Start color for the blink
    public Color endColor = Color.yellow;  // End color for the blink
    public float blinkSpeed = 2f;          // Speed of the blinking effect

    private Image buttonImage;             // Reference to the button's image

    void Start()
    {
        if (button != null)
        {
            buttonImage = button.GetComponent<Image>();
        }
        else
        {
            Debug.LogError("Button not assigned!");
        }
    }
    void Update()
    {
        if (buttonImage != null)
        {
            float t = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            buttonImage.color = Color.Lerp(startColor, endColor, t);
        }
    }

}
