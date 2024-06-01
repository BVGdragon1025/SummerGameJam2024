using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab przeciwnika
    public Transform player; // Transform gracza
    public float spawnInterval = 5f; // Czas pomiêdzy spawnowaniem przeciwników
    public int maxEnemies = 4; // Maksymalna liczba przeciwników na mapie
    public float spawnDistance = 20f; // Minimalna odleg³oœæ od gracza, w której przeciwnik mo¿e siê pojawiæ
    public float enemyHeight = 1f; // Wysokoœæ, na której pojawia siê przeciwnik

    private int currentEnemyCount = 0;

    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemy();
            }
        }
    }

    void SpawnEnemy()
    {
        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        newEnemy.GetComponent<Enemy>().Initialize(player);
        currentEnemyCount++;
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * spawnDistance;
        randomDirection += player.position;
        randomDirection.y = enemyHeight;

        return randomDirection;
    }
}
