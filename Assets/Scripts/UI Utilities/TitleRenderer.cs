using TMPro;
using UnityEngine;

public class TitleRenderer : MonoBehaviour
{
    public MenuManager MenuManager;
    [SerializeField] private float _fadeSpeed = 2f;
    private TextMeshProUGUI _pressToContinueText;
    private Color _color;

    private void Start()
    {
        _pressToContinueText = transform.Find("PressToContinueText").GetComponent<TextMeshProUGUI>();
        _color = _pressToContinueText.color;
    }

    private void FixedUpdate()
    {
        if (MenuManager.ActivePanel == MenuManager.PanelType.TitlePanel)
        {
            _color.a = Mathf.PingPong(Time.time * _fadeSpeed, 1f);
            _pressToContinueText.color = _color;
        }
    }
}
