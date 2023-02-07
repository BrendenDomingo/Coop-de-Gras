using System.Collections;
using UnityEngine;

public class FlyingEnemyAI : MonoBehaviour
{
    [SerializeField] private float health = 100f;

    public float speed = 2f;
    public GameObject projectilePrefab;
    public float fireRate = 1f;
    private float fireTimer = 0f;
    private Transform playerTransform;

    public GameObject CoinPrefab;

    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        // Move towards the player on the x-axis only
        Vector2 newPos = Vector2.MoveTowards(transform.position, new Vector2(playerTransform.position.x, transform.position.y), speed * Time.deltaTime);
        transform.position = new Vector3(newPos.x, transform.position.y, transform.position.z);

        // Fire projectile towards player's position
        fireTimer += Time.deltaTime;
        if (fireTimer >= fireRate)
        {
            fireTimer = 0f;
            Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Instantiate ( CoinPrefab, transform.position, Quaternion.identity );
            Destroy (gameObject);
        }
    }
}
