using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PickupType Type;
    private void OnCollisionEnter2D ( Collision2D collision )
    {
        if (collision.gameObject.CompareTag ( "Player" ))
        {
            Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
            collision.gameObject.GetComponent<PlayerController>().Pickup(Type);
            Destroy(gameObject);
        }
    }
}

public enum PickupType
{
    Gold,
    Egg,
    Item
}
