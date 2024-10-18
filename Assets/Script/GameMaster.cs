using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class GameMaster : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI roundsText;
    [SerializeField] private TextMeshProUGUI EnemyAliveText;
    [SerializeField] private GameObject enemyPrefab; 
    [SerializeField] private List<Transform> spawners; 

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 2f; 
    [SerializeField] private int enemiesPerRound = 5; 
    [Header("Rounds Settings")]
    public int rounds = 0;
    public int EnemyAlive = 0;
    private int log;


    private void Start()
    {
        StartCoroutine(SpawnEnemies());
        EnemyAlive++;
    }

    private IEnumerator SpawnEnemies()
    {
        SpawnEnemiesInRound();
        EnemyAlive--;
        yield return new WaitForSeconds(spawnInterval * enemiesPerRound);
    }

private void SpawnEnemiesInRound()
{
    if (spawners.Count > 0)
    {
        // Loop through the number of enemies to spawn
        for (int i = 0; i < enemiesPerRound; i++)
        {
            // Choose a random spawn point from the spawners list
            Transform spawnPoint = spawners[Random.Range(0, spawners.Count)];
            
            // Instantiate the enemy at the chosen spawn point
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            
            // Increment the count of alive enemies
            EnemyAlive++;
        }
    }
}
private void FixedUpdate()
{
    
    if (EnemyAlive <= 0)
    {
        nextround();
        Debug.Log(log++);
        EnemyAlive++;
    }
    EnemyAliveText.text = EnemyAlive.ToString();
}

    private void nextround()
    {
        rounds++;
        roundsText.text = rounds.ToString();
        enemiesPerRound++;
        StartCoroutine(SpawnEnemies());
    }
}
