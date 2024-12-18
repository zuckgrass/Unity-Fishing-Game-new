using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogicScript : MonoBehaviour
{
    public int playerScore;                // Score tracker
    public Text ScoreText;                 // UI text for displaying score
    public GameObject newGameScreen;       // Reference to the New Game screen
    public GameObject gameOverScreen;      // Reference to the Game Over screen
    public bool isGameOver = false;        // Flag to prevent updates after game over
    public MusicManager musicManager;      // Reference to the MusicManager
    public TMP_Text maxDepthText;              // Text for displaying max depth
    private HookScript hookScript;         // Reference to HookScript

    void Start()
    {
        newGameScreen = GameObject.FindGameObjectWithTag("newGameScreen");
        gameOverScreen.SetActive(false);
        ScoreText.gameObject.SetActive(false);
        hookScript = FindObjectOfType<HookScript>();
        newGameScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void StartGame()
    {
        newGameScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        ScoreText.gameObject.SetActive(true);
        Time.timeScale = 1f;
        isGameOver = false;
    }

    public void AddScore()
    {
        playerScore++;
        ScoreText.text = playerScore.ToString() + " fish";
        hookScript.IncreaseMaxDepth(playerScore);
    }

    public void GameOver(float maxDepthReached)
    {
        if (isGameOver) return;
        isGameOver = true;
        Time.timeScale = 0f;
        hookScript.ResetDepth();
        gameOverScreen.SetActive(true);
        newGameScreen.SetActive(false);

        GameObject pointsTextObject = GameObject.FindGameObjectWithTag("finalPoints");
        if (pointsTextObject != null)
        {
            TMPro.TMP_Text pointsText = pointsTextObject.GetComponent<TMPro.TMP_Text>();
            if (pointsText != null)
            {
                pointsText.text = $"HIGH SCORE: {playerScore}";
            }
        }

        maxDepthText.text = $"RECORD DEPTH: {Mathf.FloorToInt(maxDepthReached)}m";
    }

    public void RestartGame()
    {
        playerScore = 0;
        ScoreText.text = "0 fish";
        gameOverScreen.SetActive(false);
        newGameScreen.SetActive(false);
        ResetManager.Instance.ResetGame();
        Time.timeScale = 1f;
        isGameOver = false;
    }
}