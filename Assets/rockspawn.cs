using UnityEngine;
using System.Collections.Generic;

public class rockspawn : MonoBehaviour
{
    public GameObject rockPrefab;
    public float spawnRate = 1.8f;
    private float timer = 0;
    private float currentDepthY = -53.8f;
    private float depthOffsetY = 20f;
    public float depthOffsetZ = 75f;
    private float yPositionTolerance = 5f;
    private float lateralTolerance = 10f;
    public AudioSource RockSound;

    private LogicScript logic;
    private HookScript hookScript;  // Reference to HookScript
    private Transform hookTransform;
    private List<GameObject> spawnedRocks = new List<GameObject>();

    void Start()
    {
        logic = GameObject.FindObjectOfType<LogicScript>();
        hookScript = GameObject.FindObjectOfType<HookScript>();  // Find the HookScript instance
        hookTransform = GameObject.FindGameObjectWithTag("hook").transform;
    }

    void Update()
    {
        if (logic == null || logic.isGameOver) return;

        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            SpawnRock();
            currentDepthY -= depthOffsetY;
            timer = 0;
        }

        CheckRockCollisions();
    }

    void SpawnRock()
    {
        if (rockPrefab == null) return;

        float randomZ = transform.position.z + Random.Range(-depthOffsetZ, depthOffsetZ);
        Vector3 spawnPosition = new Vector3(transform.position.x, currentDepthY, randomZ);

        GameObject spawnedRock = Instantiate(rockPrefab, spawnPosition, Quaternion.identity);
        spawnedRock.tag = "rock";
        spawnedRock.transform.localScale *= 20;

        spawnedRocks.Add(spawnedRock);
    }

    void CheckRockCollisions()
    {
        if (hookTransform == null || logic == null) return;

        foreach (GameObject rock in spawnedRocks)
        {
            if (rock != null && IsWithinTolerance(rock.transform))
            {
                float maxDepthReached = hookScript.GetMaxCurrentDepth();  // Get the max depth reached from HookScript
                logic.GameOver(maxDepthReached);
                Destroy(rock);
                RockSound.Play();
            }
        }
    }

    private bool IsWithinTolerance(Transform objectTransform)
    {
        Vector3 hookTopPosition = new Vector3(hookTransform.position.x, hookTransform.position.y - 10f, hookTransform.position.z);
        float distanceX = Mathf.Abs(objectTransform.position.x - hookTopPosition.x);
        float distanceY = Mathf.Abs(objectTransform.position.y - hookTopPosition.y);
        float distanceZ = Mathf.Abs(objectTransform.position.z - hookTopPosition.z);

        bool withinTolerance = distanceY < yPositionTolerance && distanceX < lateralTolerance && distanceZ < lateralTolerance;
        return withinTolerance;
    }

    public void ResetRockSpawning()
    {
        foreach (var rock in spawnedRocks)
        {
            if (rock != null)
            {
                Destroy(rock);
            }
        }
        spawnedRocks.Clear();
        currentDepthY = -53.8f;
        timer = 0f;
        lateralTolerance = 10f;
    }
}
