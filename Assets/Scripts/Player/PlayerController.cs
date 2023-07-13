using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameManager GameManager;
    public UIManager UIManager;

    private bool canInteract = false;
    public float Health = 100f;
    public float MaxHealth = 100f;
    public int Gold = 0;
    public int Eggs = 0;
    public float Power = 100f;
    public float MaxPower = 100f;
    // eventually make this a custom list type that accepts "item" prefabs which contains images and item stats
    public List<GameObject> Items { get; private set; }
    
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _jumpForce = 5f;

    private Rigidbody2D _rigidBody;
    private bool _canJump = true;

    private void Start ( )
    {
        _rigidBody = GetComponent<Rigidbody2D> ( );
        Items = new List<GameObject> ( );
        UIManager = GameObject.FindGameObjectWithTag("UI_Manager").GetComponent<UIManager>();
    }

    private void Update()
    {
        if (GameManager.GamePaused)
        {
            _rigidBody.Sleep();
            return;
        }
        
        if (_rigidBody.IsSleeping())
        {
            _rigidBody.WakeUp();
        }
        
        _rigidBody.velocity = new Vector2(Input.GetAxis("Horizontal") * _moveSpeed, _rigidBody.velocity.y);

        if (Input.GetButtonDown("Jump") && _canJump)
        {
            Jump();
        }

        if (canInteract)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                UIManager.OpenShopPanel();
                canInteract = false;
            }

            if (Input.GetKeyDown(KeyCode.N))
            {
                GameManager.StartNextWave();
                canInteract = false;
            }
        }
    }

    private void Jump()
    {
        _rigidBody.AddForce(new Vector2(0f, _jumpForce), ForceMode2D.Impulse);
        _canJump = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _canJump = true;
        }
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            GameManager.EndGameDefeat();
        }
    }

    public void Pickup(PickupType type, object value = null)
    {
        switch (type)
        {
            case PickupType.Gold:
                Gold += (int?)value ?? 1;
                break;
            case PickupType.Egg:
                Eggs += (int?)value ?? 1;
                break;
            case PickupType.Item:
                throw new System.NotSupportedException("Item pickups not implemented");
        }
    }

    private void OnTriggerStay2D ( Collider2D other )
    {
        if (GameManager.ShopAvailable && other.CompareTag ( "Shop" ))
        {
            canInteract = true;
            UIManager.SetGameInstruction("Shopping Carriage", "Open Shop: E \n Start Next Wave: N", 0);
        }
    }

    private void OnTriggerExit2D ( Collider2D other )
    {
        if (GameManager.ShopAvailable && other.CompareTag ( "Shop" ))
        {
            canInteract = false;
            UIManager.SetGameInstruction(string.Empty, string.Empty, 0);
        }
    }
}
