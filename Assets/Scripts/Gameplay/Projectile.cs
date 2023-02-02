using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 2f;
    public float lifetime = 5f;
    private Vector2 direction;
    private float timer = 0f;
    [SerializeField] private float damage = 10f;

    private void Start()
    {
        Transform playerTransform = GameObject.FindWithTag("Player").transform;
        direction = (playerTransform.position - transform.position).normalized;
    }

    private void Update()
    {
        // Move in the set direction
        transform.Translate(direction * speed * Time.deltaTime);

        // Check if lifetime is up
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Destroy the projectile if it collides with the player
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    public void DealDamageToPlayer(GameObject player)
    {
        player.GetComponent<PlayerController>().TakeDamage(damage);
    }
}
