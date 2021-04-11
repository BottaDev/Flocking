using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class Spawner : MonoBehaviour
{
    public float spawnTime;
    public GameObject objectToSpawn;

    private float _currentSpawnTime;

    private void Start()
    {
        _currentSpawnTime = spawnTime;
    }

    private void Update()
    {
        if (objectToSpawn != null && _currentSpawnTime <= 0)
            SpawnObject();
        else
            _currentSpawnTime -= Time.deltaTime;
    }

    private void SpawnObject()
    {
        float x = Random.Range(-GameManager.instance.globalXLimit, GameManager.instance.globalXLimit);
        float z = Random.Range(-GameManager.instance.globalZLimit, GameManager.instance.globalZLimit);
        
        Instantiate(objectToSpawn, new Vector3(x, 0, z), transform.rotation);

        _currentSpawnTime = spawnTime;
    }
}
