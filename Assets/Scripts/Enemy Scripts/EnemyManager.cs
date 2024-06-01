using UnityEngine;
using System.Collections;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab przeciwnika
    public Transform player; // Transform gracza
    public float spawnInterval = 5f; // Czas pomi�dzy spawnowaniem przeciwnik�w
    public int maxEnemies = 4; // Maksymalna liczba przeciwnik�w na mapie
    public float spawnDistance = 20f; // Minimalna odleg�o�� od gracza, w kt�rej przeciwnik mo�e si� pojawi�
    public float enemyHeight = 1f; // Wysoko��, na kt�rej pojawia si� przeciwnik

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
