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
        Destroy ( gameObject );
    }
}
