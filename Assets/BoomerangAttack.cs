using UnityEngine;
using System.Collections;

public class BoomerangAttack : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private float attackRate = 1f;
    [SerializeField] private float maxDistance = 20f;

    private float nextAttackTime = 0f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            nextAttackTime = Time.time + 1f / attackRate;

            Vector3 cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            cursorPos.z = 0;

            Vector3 direction = cursorPos - transform.position;
            float distance = direction.magnitude;

            if (distance > maxDistance)
            {
                direction = direction.normalized * maxDistance;
                distance = maxDistance;
            }

            StartCoroutine(Attack(direction, distance));
        }
    }

    IEnumerator Attack(Vector3 direction, float distance)
    {
        float traveledDistance = 0f;

        while (traveledDistance < distance)
        {
            transform.position += direction.normalized * speed * Time.deltaTime;
            traveledDistance += speed * Time.deltaTime;
            yield return null;
        }

        direction = -direction;
        traveledDistance = 0f;

        while (traveledDistance < distance)
        {
            transform.position += direction.normalized * speed * Time.deltaTime;
            traveledDistance += speed * Time.deltaTime;
            yield return null;
        }
    }
}
