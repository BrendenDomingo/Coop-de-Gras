using System.Collections;
using UnityEngine;

public class FlyingEnemyAI : Enemy2D
{
    public GameObject projectilePrefab;

    void Update ( )
    {
        Move ( );
    }

    public override void Move ( )
    {
        // Move towards the player on the x-axis only
        Vector2 newPos = Vector2.MoveTowards ( transform.position, new Vector2( playerTransform.position.x, transform.position.y ), speed * Time.deltaTime );
        transform.position = new Vector3 ( newPos.x, transform.position.y, transform.position.z );

        float dist = Vector2.Distance ( playerTransform.position, transform.position );

        if ( dist < 3 && !isAttacking) { StartCoroutine ( AttackCoroutine ( ) ); }
    }

    public override void Attack ( )
    {
        // Fire projectile towards player's position
        Instantiate ( projectilePrefab, transform.position, Quaternion.identity );
    }
}
