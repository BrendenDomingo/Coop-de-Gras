using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AssetImporter : MonoBehaviour
{
    [SerializeField] private List<GameObject> _spriteReferences;
    [SerializeField] private AssetController _assetController;

    private void Start()
    {
        foreach (GameObject gameObject in _spriteReferences)
        {
            if (gameObject.name.ToLower().Contains("background"))
            {
                HandleBackground(gameObject);
                continue;
            }

            Slider sliderComponent = gameObject.GetComponent<Slider>();
            if (sliderComponent != null)
            {
                HandleSlider(sliderComponent);
                continue;
            }

            Button buttonComponent = gameObject.GetComponent<Button>();
            if (buttonComponent != null)
            {
                HandleButton(buttonComponent);
                continue;
            }

            TextMeshProUGUI textComponent = gameObject.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                HandleTextMeshProUGUI(textComponent, gameObject);
                continue;
            }
        }
    }

    private void HandleBackground(GameObject gameObject)
    {
        Image image = gameObject.GetComponent<Image>();

        if (gameObject.name.ToLower().Contains("mainmenu"))
        {
            image.sprite = _assetController.MainMenuBackgroundSprite;
            image.enabled = true;
        }
        else if (gameObject.name.ToLower().Contains("inventory"))
        {
            image.color = new Color(_assetController.EmptyUIColor.r, _assetController.EmptyUIColor.g, _assetController.EmptyUIColor.b, 0.5f);
        }
    }

    private void HandleSlider(Slider component)
    {
        Transform background;
        Transform fillArea;

        bool isHPSlider = component.gameObject.name.Contains("HP");
        bool isPWRSlider = component.gameObject.name.Contains("HP");

        if (_assetController.UseSliderSprites && !isHPSlider && !isPWRSlider)
        {
            component.image.sprite = _assetController.SliderKnobSprite;

            background = component.transform.Find("Background");
            if (background != null)
            {
                background.GetComponent<Image>().sprite = _assetController.SliderBackgroundSprite;
            }

            fillArea = component.transform.Find("Fill Area").transform.Find("Fill");
            if (fillArea != null)
            {
                fillArea.GetComponent<Image>().sprite = _assetController.SliderFillAreaSprite;
            }
            return;
        }

        ColorBlock colors = component.colors;
        colors.normalColor = _assetController.NormalUIColor;
        colors.highlightedColor = _assetController.HighlightedUIColor;
        colors.pressedColor = _assetController.PressedUIColor;
        colors.selectedColor = _assetController.SelectedUIColor;
        colors.disabledColor = _assetController.DisabledUIColor;
        component.colors = colors;

        fillArea = component.transform.Find("Fill Area").transform.Find("Fill");
        fillArea.GetComponent<Image>().color = isHPSlider ? 
            _assetController.HPColor :
            isPWRSlider ?
                _assetController.PWRColor :
                _assetController.FillUIColor;

        background = component.transform.Find("Background");
        background.GetComponent<Image>().color = _assetController.EmptyUIColor;
    }
    
    private void HandleButton(Button component)
    {
        if (_assetController.UseButtonSprite)
        {
            component.image.sprite = _assetController.SliderKnobSprite;
            return;
        }

        ColorBlock colors = component.colors;
        colors.normalColor = _assetController.NormalUIColor;
        colors.highlightedColor = _assetController.HighlightedUIColor;
        colors.pressedColor = _assetController.PressedUIColor;
        colors.selectedColor = _assetController.SelectedUIColor;
        colors.disabledColor = _assetController.DisabledUIColor;
        component.colors = colors;

        Transform text = component.transform.Find("Text (TMP)");
        text.GetComponent<TextMeshProUGUI>().font = _assetController.BodyFont;
    }

    private void HandleTextMeshProUGUI(TextMeshProUGUI component, GameObject gameObject)
    {
        if (gameObject.name.ToLower().Contains("title") || gameObject.name.ToLower().Contains("header"))
        {
            component.font = _assetController.HeaderFont;
            return;
        }

        component.font = _assetController.BodyFont;
    }
}
