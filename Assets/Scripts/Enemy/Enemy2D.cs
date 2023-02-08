using UnityEngine;

public class Enemy2D : MonoBehaviour
{
    public float health;
    public float speed;
    public float attackRate;
    public float damage;
    public Transform playerTransform;
    public GameObject itemDrop;

    public virtual void ReceiveDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Death effect or animation here
        if (itemDrop != null)
        {
            Instantiate(itemDrop, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }
}
