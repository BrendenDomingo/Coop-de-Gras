using System.Collections;
using UnityEngine;

public class FlyingEnemyAI : Enemy2D
{
    private bool isAttacking;
    public GameObject projectilePrefab;

    void Update()
    {
        MoveTowardsTarget();
    }

    private void OnCollisionStay2D ( Collision2D collision )
    {
        if (collision.gameObject.CompareTag ( "Player" ) && !isAttacking)
        {
            StartCoroutine ( AttackCoroutine ( collision.gameObject ) );
        }
    }

    private IEnumerator AttackCoroutine ( GameObject player )
    {
        isAttacking = true;
        FireProjectile ( );
        yield return new WaitForSeconds ( attackRate );
        isAttacking = false;
    }

    private void MoveTowardsTarget()
    {
        // Move towards the player on the x-axis only
        Vector2 newPos = Vector2.MoveTowards (transform.position, new Vector2(playerTransform.position.x, transform.position.y), speed * Time.deltaTime);
        transform.position = new Vector3 (newPos.x, transform.position.y, transform.position.z);
    }

    private void FireProjectile()
    {
        // Fire projectile towards player's position
        Instantiate (projectilePrefab, transform.position, Quaternion.identity);
    }

}
