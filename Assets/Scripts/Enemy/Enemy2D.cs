using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2D : MonoBehaviour
{
    public float health;
    public float speed;
    public float attackRate;
    public float damage;
    public Transform playerTransform;
    public GameObject player;
    public GameObject itemDrop;

    public int coinsToDrop; // the number of coins to drop upon death
    public float coinSpeed; // the speed at which the coins will be shot out
    public GameObject coinPrefab; // the coin prefab to be instantiated
    public LayerMask enemyLayer; // the layer(s) to collide with the environment

    public bool isAttacking;

    public virtual void ReceiveDamage ( float damage )
    {
        health -= damage;
        if (health <= 0)
        {
            Die ( );
        }
    }

    public virtual void Move ( ) { }

    public virtual void Attack ( ) { }

    public virtual IEnumerator AttackCoroutine ( )
    {
        isAttacking = true;
        Attack ( );
        yield return new WaitForSeconds ( attackRate );
        isAttacking = false;
    }

    void Die ( )
    {
        // Death effect or animation here
        if ( itemDrop != null )
        {
            Instantiate( itemDrop, transform.position, transform.rotation );
        }

        // Create and shoot out coins
        for (int i = 0; i < coinsToDrop; i++)
        {
            Vector2 randomDirection = new Vector2 ( Random.Range ( -1f, 1f ), Random.Range ( 0f, 1f ) ).normalized;
            float randomSpeed = Random.Range ( coinSpeed * 0.5f, coinSpeed * 1.5f );
            GameObject coin = Instantiate ( coinPrefab, transform.position, Quaternion.identity );

            // Set the layer of the coin to "Coins" to exclude it from colliding with the "Enemy" layer
            coin.layer = LayerMask.NameToLayer ( "Coins" );

            Rigidbody2D coinRigidbody = coin.GetComponent<Rigidbody2D> ( );
            coinRigidbody.AddForce ( randomDirection * randomSpeed, ForceMode2D.Impulse );

            // Ignore collisions with the enemy's collider
            Physics2D.IgnoreCollision ( coin.GetComponent<Collider2D> ( ), GetComponent<Collider2D> ( ) );

            // Ignore collisions with other coins
            Physics2D.IgnoreLayerCollision ( coin.layer, coin.layer );

            // Ignore collision between the "Enemy" layer and the "Coins" layer
            Physics2D.IgnoreLayerCollision ( LayerMask.NameToLayer ( "enemy" ), LayerMask.NameToLayer ( "Coins" ), true );
        }

        // Record death 
        try 
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().EnemyKilled();
        }
        catch
        {
            Debug.LogError("Enemy2D.cs: GameManager not found to record enemy death!");
        }
        
        Destroy ( gameObject );
    }
}
