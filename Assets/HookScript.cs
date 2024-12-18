using UnityEngine;
using UnityEngine.UI;

public class HookScript : MonoBehaviour
{
    public float fallSpeed;             // Speed for downward movement
    public float horizontalSpeed;       // Speed for left-right movement
    public int score = 0;               // Current score
    public float maxDepth = 400f;       // Maximum depth available
    public Text depthText;              // Reference to depth UI

    private float currentDepth = 0f;    // Current depth of the hook
    private float maxCurrentDepth = 0f; // Track the maximum depth reached
    private LogicScript logicScript;    // Reference to LogicScript

    void Start()
    {
        depthText.gameObject.SetActive(true);
        logicScript = FindObjectOfType<LogicScript>();
        UpdateDepthUI();
    }

    void Update()
    {
        if (logicScript.isGameOver) return;

        // Move hook downwards and track depth
        float distance = fallSpeed * Time.deltaTime;
        transform.Translate(Vector3.down * distance);
        currentDepth += distance;

        // Update max depth reached
        if (currentDepth > maxCurrentDepth)
        {
            maxCurrentDepth = currentDepth;
        }

        // Check if hook has reached max depth
        if (currentDepth >= maxDepth)
        {
            logicScript.GameOver(maxCurrentDepth);
        }

        UpdateDepthUI();

        // Horizontal movement
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * horizontalSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * horizontalSpeed * Time.deltaTime);
        }
    }

    // Increase max depth based on score
    public void IncreaseMaxDepth(int playerScore)
    {
        int scoreRange = (playerScore / 10) * 10;
        float depthIncrement = scoreRange + 10f;
        maxDepth += depthIncrement;
    }

    // Update depth UI to show remaining depth
    void UpdateDepthUI()
    {
        float depthRemaining = maxDepth - currentDepth;
        depthText.text = $"{Mathf.Max(0, Mathf.Ceil(depthRemaining))}m";
    }

    // Reset depth when restarting
    public void ResetDepth()
    {
        currentDepth = 0f;
        maxCurrentDepth = 0f;
        UpdateDepthUI();
    }

    public float GetMaxCurrentDepth() => maxCurrentDepth;
}
