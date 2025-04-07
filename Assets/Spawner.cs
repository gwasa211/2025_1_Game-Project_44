using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject coinPrefabs;
    public GameObject Missileprfabs;

    [Header("스폰 타이밍 설정")]
    public float minSpawnInterval = 0.5f;
    public float maxSawnInterval = 2.0f;

    [Header("동전 스폰 확률 설정")]
    [Range(0, 100)]
    public int coinSpawnChance = 50;


    public float timer = 0.0f;
    public float nextSpawnTime;
    // Start is called before the first frame update
    void Start()
    {
        SetNestSpawnTime();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(timer > nextSpawnTime)
        {
            SpawnObject();
            timer = 0.0f;
            SetNestSpawnTime();
        }
    }
    void SetNestSpawnTime()
    {
        nextSpawnTime = Random.Range(minSpawnInterval, maxSawnInterval);
    }
    void SpawnObject()
    {
        Transform spawnTransform = transform;

        int randomValue = Random.Range(0, 100);
        if (randomValue < coinSpawnChance) 
        {
            Instantiate(coinPrefabs, spawnTransform.position, spawnTransform.rotation);
        }
        else
        {
            Instantiate(Missileprfabs, spawnTransform.position, spawnTransform.rotation);
        }
    }
}
