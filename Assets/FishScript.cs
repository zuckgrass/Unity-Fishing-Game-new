using UnityEngine;
using System.Collections.Generic;

public class FishMovement : MonoBehaviour
{
    public GameObject GoldFish;
    public GameObject BlueFish;
    public float spawnRate = 1.8f;
    public float depthOffsetY = 20f;
    public float depthOffsetZ = 75f;
    public AudioSource bubble;

    private float timer = 0;
    private float currentDepthY = -53.8f;
    private float yPositionTolerance = 5f;
    private float lateralTolerance = 10f;
    private Transform hookTransform;
    private List<GameObject> spawnedFish = new List<GameObject>();
    private LogicScript logic;
    private GameObject[] fishArray;

    void Start()
    {
        logic = GameObject.FindObjectOfType<LogicScript>();
        hookTransform = GameObject.FindGameObjectWithTag("hook").transform;
        fishArray = new GameObject[] { GoldFish, BlueFish };
    }

    void Update()
    {
        if (logic == null || logic.isGameOver) return;
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            SpawnFish();
            currentDepthY -= depthOffsetY;
            timer = 0;
        }
        CheckFishCatches();
    }

    void SpawnFish()
    {
        int randomIndex = Random.Range(0, fishArray.Length);
        GameObject fishPrefab = fishArray[randomIndex];

        float randomZ = transform.position.z + Random.Range(-depthOffsetZ, depthOffsetZ);
        Vector3 spawnPosition = new Vector3(transform.position.x, currentDepthY, randomZ);
        GameObject spawnedFishObj = Instantiate(fishPrefab, spawnPosition, Quaternion.identity);

        if (fishPrefab == BlueFish)
        {
            spawnedFishObj.transform.Rotate(90f, 0f, -25f);
            spawnedFishObj.transform.localScale /= 1.5f;
            spawnedFishObj.transform.position += new Vector3(0f, 0f, -10f);
        }
        else
        {
            spawnedFishObj.transform.localScale *= 5f;
        }

        spawnedFishObj.tag = "fish";
        spawnedFish.Add(spawnedFishObj);
    }

    void CheckFishCatches()
    {
        for (int i = spawnedFish.Count - 1; i >= 0; i--)
        {
            if (spawnedFish[i] != null && IsWithinTolerance(spawnedFish[i].transform))
            {
                logic.AddScore();
                bubble.Play();
                Destroy(spawnedFish[i]);
                spawnedFish.RemoveAt(i);
            }
        }
    }

    private bool IsWithinTolerance(Transform objectTransform)
    {
        Vector3 hookPosition = new Vector3(hookTransform.position.x, hookTransform.position.y - 10f, hookTransform.position.z);
        float distanceX = Mathf.Abs(objectTransform.position.x - hookPosition.x);
        float distanceY = Mathf.Abs(objectTransform.position.y - hookPosition.y);
        float distanceZ = Mathf.Abs(objectTransform.position.z - hookPosition.z);
        return distanceY < yPositionTolerance && distanceX < lateralTolerance && distanceZ < lateralTolerance;
    }

    public void ResetFishSpawning()
    {
        foreach (var fish in spawnedFish)
        {
            if (fish != null) Destroy(fish);
        }
        spawnedFish.Clear();
        currentDepthY = -53.8f;
        timer = 0f;
    }
    
}
