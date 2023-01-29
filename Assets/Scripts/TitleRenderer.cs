using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleRenderer : MonoBehaviour
{
    public MenuManager MenuManager;
    [SerializeField] private float _fadeSpeed = 2f;
    private TextMeshProUGUI _pressToContinueText;
    private Color _color;
    private bool _fadeIn, _fadeOut;

    void Start()
    {
        _pressToContinueText = transform.Find("PressToContinueText").GetComponent<TextMeshProUGUI>();
        _color = _pressToContinueText.color;
        _fadeIn = true;
        _fadeOut = false;
    }

    void FixedUpdate()
    {
        if (MenuManager.ActivePanel == MenuManager.PanelType.TitlePanel)
        {
            if (_fadeIn)
            {
                if (_color.a < 1.0f)
                {
                    _color.a += (Time.deltaTime * _fadeSpeed);
                    _pressToContinueText.color = _color;
                }
                else
                {
                    _fadeIn = false;
                    _fadeOut = true;
                } 
            }
            else if (_fadeOut)
            {
                if (_color.a > 0.0f)
                {
                    _color.a -= (Time.deltaTime * _fadeSpeed);
                    _pressToContinueText.color = _color;
                }
                else
                {
                    _fadeIn = true;
                    _fadeOut = false;
                }  
            }
        }
    }
}