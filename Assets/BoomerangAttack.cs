using UnityEngine;
using System.Collections;

public class BoomerangAttack : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private float maxDistance = 20f;
    [SerializeField] private float damage = 10f;
    private float nextAttackTime = 0f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + 1f / attackRate;

            Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorPos.z = 0;

            Vector3 direction = cursorPos - transform.position;

            if (direction.magnitude > maxDistance)
            {
                direction = direction.normalized * maxDistance;
            }

            StartCoroutine(Attack(direction));
        }
    }

    IEnumerator Attack(Vector3 direction)
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
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<EnemyAI>().TakeDamage(damage);
        }
    }
}