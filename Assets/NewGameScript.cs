using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameStartManager : MonoBehaviour
{
    private GameObject newGameScreen;
    private Button newGameButton;
    public Color startColor = Color.red;
    public Color endColor = Color.yellow;
    public float blinkSpeed = 1f;
    private Image buttonImage;

    private Button exitButton; // Reference to the Exit button

    void Start()
    {
        Debug.Log("GameStartManager: Start called");

        // Initialize New Game Screen
        newGameScreen = GameObject.FindGameObjectWithTag("newGameScreen");
        if (newGameScreen != null)
        {
            newGameScreen.SetActive(true);
            Debug.Log("New game screen activated");
        }
        else
        {
            Debug.LogError("New game screen not found!");
        }

        // Initialize New Game Button
        GameObject buttonObject = GameObject.FindGameObjectWithTag("newGame");
        if (buttonObject != null)
        {
            Debug.Log("Button found!");
            newGameButton = buttonObject.GetComponent<Button>();
            buttonImage = buttonObject.GetComponent<Image>();

            if (buttonImage == null)
            {
                Debug.LogError("Button does not have an Image component!");
            }
            else
            {
                Debug.Log("Button Image Component Found!");
                StartCoroutine(BlinkButton());
            }
            newGameButton.onClick.AddListener(OnNewGamePressed);
        }
        else
        {
            Debug.LogError("New game button not found!");
        }

        // Initialize Exit Button
        GameObject exitButtonObject = GameObject.FindGameObjectWithTag("exit");
        if (exitButtonObject != null)
        {
            Debug.Log("Exit button found!");
            exitButton = exitButtonObject.GetComponent<Button>();
            exitButton.onClick.AddListener(OnExitPressed);
        }
        else
        {
            Debug.LogError("Exit button not found!");
        }
    }

    void OnNewGamePressed()
    {
        if (newGameScreen != null)
        {
            newGameScreen.SetActive(false);
            Time.timeScale = 1f;
        }

        LogicScript logicScript = FindObjectOfType<LogicScript>();
        if (logicScript != null)
        {
            logicScript.StartGame();
        }
    }

    void OnExitPressed()
    {
        Debug.Log("Exit Button Pressed!");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // For testing in the Unity Editor
#else
        Application.Quit(); // For builds
#endif
    }

    private IEnumerator BlinkButton()
    {
        while (true)
        {
            float t = Mathf.PingPong(Time.time * blinkSpeed, 1f);
            Color targetColor = Color.Lerp(startColor, endColor, t);
            buttonImage.color = targetColor;
            yield return null;
        }
    }
}
