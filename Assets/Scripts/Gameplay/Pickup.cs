using UnityEngine;

public class Pickup : MonoBehaviour
{
    private void OnCollisionEnter2D ( Collision2D collision )
    {
        if (collision.gameObject.CompareTag ( "Player" ))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController> ( );
            if (player != null)
            {
                player.PickupGold ( );
            }
            Destroy ( gameObject );
        }
    }
}
