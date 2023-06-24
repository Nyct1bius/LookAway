using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private Transform player;

    [SerializeField] private GameObject enemy;

    public bool canSpawnEnemies = false;

    [SerializeField] private LayerMask terrainMask;

    [SerializeField] private float spawnAreaRadiusMax, spawnAreaRadiusMin, spawnCooldownMax;
    private float spawnCooldown;

    private Vector3 spawnPoint;

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

        if (spawnCooldown <= 0 )
        {
            FindSpawnPoint();
        }
    }

    void FindSpawnPoint()
    {
        float randomX = Random.Range(-spawnAreaRadiusMax, spawnAreaRadiusMax);
        float randomZ = Random.Range(-spawnAreaRadiusMax, spawnAreaRadiusMax);

        spawnPoint = new Vector3(transform.position.x + randomX, transform.position.y + 4, transform.position.z + randomZ);;

        if (canSpawnEnemies && Physics.Raycast(spawnPoint, -transform.up, 5f, terrainMask) && !Physics.Raycast(spawnPoint, -transform.up, 5f, LayerMask.GetMask("Default")) && Vector3.Distance(transform.position, spawnPoint) >= spawnAreaRadiusMin)
        {
            Instantiate(enemy, spawnPoint, Quaternion.identity);
            spawnCooldown = spawnCooldownMax;
        }
    }
}
