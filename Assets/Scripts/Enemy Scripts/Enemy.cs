using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 5f; // Pr�dko�� poruszania si� przeciwnika
    private Transform player; // Transform gracza
    private bool isInitialized = false; // Flaga inicjalizacji

    // Zmienna, kt�ra zostanie zmieniona po kolizji
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
            // Tutaj mo�na zmieni� inn� zmienn� lub wywo�a� jak�� akcj�
            Destroy(gameObject); // Zniszcz przeciwnika po kolizji z graczem
        }
    }
}
