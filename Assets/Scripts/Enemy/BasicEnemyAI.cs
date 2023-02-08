using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAI : Enemy2D
{
    private bool isAttacking;

    private void Update()
    {
        MoveTowardsTarget();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isAttacking)
        {
            StartCoroutine (AttackCoroutine(collision.gameObject));
        }
    }

    private IEnumerator AttackCoroutine (GameObject player)
    {
        isAttacking = true;
        DealDamageToPlayer (player);
        yield return new WaitForSeconds (attackRate);
        isAttacking = false;
    }

    private void MoveTowardsTarget()
    {
        Vector3 targetDirection = playerTransform.position - transform.position;
        transform.position = Vector2.MoveTowards (transform.position, playerTransform.position, speed * Time.deltaTime);
    }

    public void DealDamageToPlayer (GameObject player)
    {
        player.GetComponent<PlayerController>().TakeDamage(damage);
    }
}
