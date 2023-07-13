using UnityEngine;
using System.Collections;

public class BoomerangAttack : MonoBehaviour
{
    public float speed;
    public float attackRate;
    public float maxDistance;
    public float damage;
    private float nextAttackTime;
    public bool isAttacking = false;

    public float radius;
    public float rotationSpeed;

    private void Update()
    {
        if (GameManager.GamePaused) return;

        Vector3 cursorPos = Camera.main.ScreenToWorldPoint ( Input.mousePosition );
        cursorPos.z = 0;

        Vector3 direction = cursorPos - transform.position;

        if (direction.magnitude > maxDistance)
        {
            direction = direction.normalized * maxDistance;
        }

        Vector3 directionClamped = Vector3.ClampMagnitude ( direction, 1f );

        Quaternion targetRotation = Quaternion.LookRotation ( Vector3.forward, direction ) * Quaternion.Euler ( 0, 0, 90 );
        transform.rotation = targetRotation;


        if (!isAttacking)
        {
            transform.position = transform.parent.position + directionClamped.normalized;
        }

        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            isAttacking = true;
            nextAttackTime = (Time.time + 1f) / attackRate;

            StartCoroutine(Attack(direction));
        }
    }

    private IEnumerator Attack(Vector3 direction)
    {
        float traveledDistance = 0f;

        while (traveledDistance < direction.magnitude)
        {
            transform.position += direction.normalized * speed * Time.deltaTime;
            traveledDistance += speed * Time.deltaTime;
            yield return null;
        }

        direction = -direction;
        traveledDistance = 0f;

        while (traveledDistance < direction.magnitude)
        {
            transform.position += direction.normalized * speed * Time.deltaTime;
            traveledDistance += speed * Time.deltaTime;
            yield return null;
        }

        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            BasicEnemyAI enemyAI = other.gameObject.GetComponent<BasicEnemyAI>();
            if (enemyAI != null)
            {
                enemyAI.ReceiveDamage ( damage);
            }

            FlyingEnemyAI flyingEnemyAI = other.gameObject.GetComponent<FlyingEnemyAI>();
            if (flyingEnemyAI != null)
            {
                flyingEnemyAI.ReceiveDamage ( damage);
            }
        }
    }
}
