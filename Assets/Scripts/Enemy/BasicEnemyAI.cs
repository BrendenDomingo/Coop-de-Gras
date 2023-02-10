using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyAI : Enemy2D
{
    private void Update ( )
    {
        Move ( );
    }

    private void OnCollisionStay2D ( Collision2D collision )
    {
        if ( collision.gameObject.CompareTag( "Player" ) && !isAttacking )
        {
            StartCoroutine ( AttackCoroutine ( ) );
        }
    }

    public override void Move ( )
    {
        Vector3 targetDirection = playerTransform.position - transform.position;
        transform.position = Vector2.MoveTowards ( transform.position, playerTransform.position, speed * Time.deltaTime );
    }

    public override void Attack ( )
    {
        player.GetComponent<PlayerController> ( ).TakeDamage( damage );
    }
}
