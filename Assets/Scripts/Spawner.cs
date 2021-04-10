using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float spawnTime;
    public GameObject objectToSpawn;

    private float _currentSpawnTime;
    
    private void Update()
    {
        if (objectToSpawn != null && _currentSpawnTime <= 0)
            SpawnObject();
        else
            _currentSpawnTime -= Time.deltaTime;
    }

    private void SpawnObject()
    {
        Vector3 position = new Vector3();

        Instantiate(objectToSpawn, position, transform.rotation);

        _currentSpawnTime = spawnTime;
    }
}
