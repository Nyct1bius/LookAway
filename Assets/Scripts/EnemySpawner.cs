using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private Transform playerCamera;

    [SerializeField] private GameObject enemy;

    public bool canSpawnEnemies = false;

    [SerializeField] private LayerMask terrainMask;

    [SerializeField] private float spawnRadiusMin, spawnRadiusMax, spawnCooldownMax;
    private float spawnCooldown;    

    private Vector3 spawnPoint;

    private bool spawnPointSet = false;
    
    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;

        spawnCooldown = spawnCooldownMax;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerCamera.position;
        
        spawnCooldown -= Time.deltaTime;
        
        if (spawnCooldown <= 0)
        {
            FindSpawnPoint();
        }
    }

    void FindSpawnPoint()
    {
        float randomX = Random.Range(-spawnRadiusMax - -spawnRadiusMin, spawnRadiusMax - spawnRadiusMin);
        float randomZ = Random.Range(-spawnRadiusMax - -spawnRadiusMin, spawnRadiusMax - spawnRadiusMin);

        spawnPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(spawnPoint, transform.up, 2f, terrainMask) && Physics.Raycast(spawnPoint, -transform.up, 2f, terrainMask))
        {
            //Instantiate(enemy, spawnPoint, Quaternion.identity);
            spawnCooldown = spawnCooldownMax;
        }
    }
}
