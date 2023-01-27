using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public Transform chaseTarget;
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    public float health;
    public float damageOverTime = 1f;
    private float damageTimer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        damageTimer = damageOverTime;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetDirection = chaseTarget.position - transform.position;
        
        transform.position = Vector2.MoveTowards(transform.position, chaseTarget.position, moveSpeed * Time.deltaTime);

        if (damageTimer > 0)
        {
            damageTimer -= Time.deltaTime;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (damageTimer <= 0)
            {
                collision.gameObject.GetComponent<PlayerController>().TakeDamage();
                damageTimer = damageOverTime;
            }
        }
    }

    public void TakeDamage()
    {
        health--;

        if (health == 0)
        {
            Destroy(gameObject);
        }
    }
}
