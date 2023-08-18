using UnityEngine;

public class Shop : MonoBehaviour
{
    public KeyCode[] interactionKeys = { KeyCode.Space, KeyCode.E };

    private bool canInteract = false;

    private GameObject gameManager;
    private MonoBehaviour GMScript;
    private GameObject UIManager;
    private MonoBehaviour UIScript;

    private void Start ( )
    {
        gameManager = GameObject.FindWithTag ( "GameManager" );
        GMScript = gameManager.GetComponent ( "GameManager" ) as MonoBehaviour;
        UIManager = GameObject.FindWithTag ( "UI_Manager" );
        UIScript = UIManager.GetComponent ( "UI Manager" ) as MonoBehaviour;

        Debug.Log ( "BAM" );
    }

    private void OnTriggerStay ( Collider other )
    {

        Debug.Log ( "TRUE" );
        if (other.CompareTag ( "Player" ))
        {
            
            canInteract = true;
            
        }
    }

    private void OnTriggerExit ( Collider other )
    {
        if (other.CompareTag ( "Player" ))
        {
            
            canInteract = false;
        }
    }

    private void OnGUI ( )
    {
        if (canInteract)
        {
            GUI.Label ( new Rect ( Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50 ), "Space: Next Wave  E: Shop" );
            foreach (KeyCode key in interactionKeys)
            {
                if (Input.GetKeyDown ( key ) && key == KeyCode.Space)
                {
                    if (GMScript != null)
                    {
                        //GMScript.Invoke ( NextWave, 0f );
                    }
                }
                if (Input.GetKeyDown ( key ) && key == KeyCode.E)
                {
                    //UIManager.OpenShopPanel ( );
                }
            }
        }
    }
}

