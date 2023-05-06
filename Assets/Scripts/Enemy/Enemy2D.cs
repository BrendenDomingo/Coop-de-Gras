using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy2D : MonoBehaviour
{
    public float health;
    public float speed;
    public float attackRate;
    public float damage;
    public Transform playerTransform;
    public GameObject player;
    public List<ItemDropRange> drops;
    private float _itemDropSpeed = 2f; // the speed at which the items will be shot out
    public LayerMask enemyLayer; // the layer(s) to collide with the environment
    public Slider hpSlider;
    private float _maxHealth;

    public bool isAttacking;

    void Start()
    {
        _maxHealth = health;
    }

    public virtual void ReceiveDamage ( float damage )
    {
        health -= damage;
        hpSlider.value = health / _maxHealth;
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
        yield return new WaitForSeconds ( 1 / attackRate );
        isAttacking = false;
    }

    void Die ( )
    {
        // Death effect or animation here
        if ( drops != null && drops.Count > 0 )
        {
            float random;
            float dropLaunchSpeed;
            int itemLayer = LayerMask.NameToLayer ( "Items" );
            int enemyLayer = LayerMask.NameToLayer ( "enemy" );

            foreach (ItemDropRange item in drops)
            {
                random = Random.Range( 0f, 100f );
                if (item.DropChance >= random)
                {
                    random = Random.Range(item.DropCountMinimum, item.DropCountMaximum);
                    // Create and shoot out the item(s)
                    for (int i = 0; i < random; i++)
                    {
                        Vector2 randomDirection = new Vector2 ( Random.Range ( -1f, 1f ), Random.Range ( 0f, 1f ) ).normalized;
                        dropLaunchSpeed = Random.Range ( _itemDropSpeed * 0.5f, _itemDropSpeed * 1.5f );
                        GameObject drop = Instantiate ( item.ItemPrefab, transform.position, Quaternion.identity );

                        // Set the layer of the coin to "Coins" to exclude it from colliding with the "Enemy" layer
                        drop.layer = itemLayer;

                        Rigidbody2D coinRigidbody = drop.GetComponent<Rigidbody2D> ( );
                        coinRigidbody.AddForce ( randomDirection * dropLaunchSpeed, ForceMode2D.Impulse );

                        // Ignore collisions with the enemy's collider
                        Physics2D.IgnoreCollision ( drop.GetComponent<Collider2D> ( ), GetComponent<Collider2D> ( ) );

                        // Ignore collisions with other coins
                        Physics2D.IgnoreLayerCollision ( drop.layer, drop.layer );

                        // Ignore collision between the "Enemy" layer and the "Coins" layer
                        Physics2D.IgnoreLayerCollision ( enemyLayer, itemLayer, true );
                    }

                }
            }
            
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

[System.Serializable]
public struct ItemDropRange
{
    public GameObject ItemPrefab;
    /// <summary> Drop chance of the item (0f-100f) </summary>
    public float DropChance;
    /// <summary> Minimum number of drops on successful drop roll (>=0) </summary>
    public int DropCountMinimum;
    /// <summary> Maximum number of drops on successful drop roll (uniform distribution) </summary>
    public int DropCountMaximum; 
    /// <summary> Maximum number of drops on successful drop roll (uniform distribution) </summary>
}
