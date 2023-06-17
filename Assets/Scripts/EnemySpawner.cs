using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private Transform player;

    [SerializeField] private GameObject enemy;

    public bool canSpawnEnemies = false;

    [SerializeField] private LayerMask terrainMask;

    [SerializeField] private float spawnAreaRadius, spawnCooldownMax;
    private float spawnCooldown;

    private Vector3 spawnPoint;

    //private bool spawnPointSet = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        spawnCooldown = spawnCooldownMax;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position;

        if (canSpawnEnemies)
        {
            spawnCooldown -= Time.deltaTime;
        }

        if (spawnCooldown <= 0)
        {
            FindSpawnPoint();
        }
    }

    void FindSpawnPoint()
    {
        float randomX = Random.Range(-spawnAreaRadius, spawnAreaRadius);
        float randomZ = Random.Range(-spawnAreaRadius, spawnAreaRadius);

        spawnPoint = new Vector3(transform.position.x + randomX, transform.position.y + 4, transform.position.z + randomZ);

        if (Physics.Raycast(spawnPoint, -transform.up, 5f, terrainMask) && canSpawnEnemies && !Physics.Raycast(spawnPoint, -transform.up, 5f, LayerMask.GetMask("Default")) && Vector3.Distance(transform.position, spawnPoint) >= 40)
        {
            Instantiate(enemy, spawnPoint, Quaternion.identity);
            Debug.Log("Enemy Spawned");
            spawnCooldown = spawnCooldownMax;
        }
    }
}
