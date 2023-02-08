using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform chaseTarget;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float health = 100f;
    [SerializeField] private float attackCooldown = 15f;
    [SerializeField] private float damage = 10f;

    public GameObject CoinPrefab;

    private Rigidbody2D rb;
    private bool isAttacking;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        MoveTowardsTarget();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isAttacking)
        {
            StartCoroutine(AttackCoroutine(collision.gameObject));
        }
    }

    private IEnumerator AttackCoroutine(GameObject player)
    {
        isAttacking = true;
        DealDamageToPlayer(player);
        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }

    private void MoveTowardsTarget()
    {
        Vector3 targetDirection = chaseTarget.position - transform.position;
        transform.position = Vector2.MoveTowards(transform.position, chaseTarget.position, moveSpeed * Time.deltaTime);
    }

    public void DealDamageToPlayer(GameObject player)
    {
        player.GetComponent<PlayerController>().TakeDamage(damage);
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
