using UnityEngine;
using System.Collections;

public class BoomerangAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public float speed;
    public float attackRate;
    public float maxDistance;
    public float damage;

    [Header("Rotation Settings")]
    public float rotationSpeed;

    private float nextAttackTime;
    private float traveledDistance;
    private bool isAttacking = false;
    private Vector3 targetPosition;

    private void Update()
    {
        if (GameManager.GamePaused || isAttacking) return;

        Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPos.z = 0;

        Vector3 direction = cursorPos - transform.position;

        if (direction.magnitude > maxDistance)
        {
            direction = direction.normalized * maxDistance;
        }
        Vector3 directionClamped = Vector3.ClampMagnitude(direction, 1f);

        if (transform.position == transform.parent.position)
        {
            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, direction) * Quaternion.Euler(0, 0, 90);
            transform.rotation = targetRotation;
        }
        
        targetPosition = transform.parent.position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);


        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            isAttacking = true;
            nextAttackTime = Time.time + 1f / attackRate;

            StartCoroutine(Attack(direction));
        }
    }

    private IEnumerator Attack(Vector3 direction)
    {
        traveledDistance = 0f;
        targetPosition = transform.parent.position + direction;
        

        while (traveledDistance < maxDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
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
                enemyAI.ReceiveDamage(damage);
            }

            FlyingEnemyAI flyingEnemyAI = other.gameObject.GetComponent<FlyingEnemyAI>();
            if (flyingEnemyAI != null)
            {
                flyingEnemyAI.ReceiveDamage(damage);
            }
        }
    }
}
