using UnityEngine;

public class BasicEnemyAI : Enemy2D
{
    private void Update ( )
    {
        if (GameManager.GamePaused) return;
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
        if (GameManager.GamePaused) return;

        player.GetComponent<PlayerController> ( ).TakeDamage( damage );
    }
}
