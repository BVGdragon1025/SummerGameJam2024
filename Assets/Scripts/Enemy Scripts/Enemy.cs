using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 5f; // Prêdkoœæ poruszania siê przeciwnika
    private Transform player; // Transform gracza
    private bool isInitialized = false; // Flaga inicjalizacji

    // Zmienna, która zostanie zmieniona po kolizji
    public bool hasCollidedWithPlayer = false;

    public void Initialize(Transform playerTransform)
    {
        player = playerTransform;
        isInitialized = true;
    }

    void Update()
    {
        if (isInitialized)
        {
            MoveTowardsPlayer();
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            hasCollidedWithPlayer = true;
            Debug.Log("Enemy collided with player!");
            // Tutaj mo¿na zmieniæ inn¹ zmienn¹ lub wywo³aæ jak¹œ akcjê
            Destroy(gameObject); // Zniszcz przeciwnika po kolizji z graczem
        }
    }
}
