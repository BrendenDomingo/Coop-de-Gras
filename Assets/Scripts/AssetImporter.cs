using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AssetImporter : MonoBehaviour
{
    [SerializeField] private List<GameObject> _spriteReferences;
    [SerializeField] private AssetController _assetController;

    void Start()
    {
        foreach (GameObject gameObject in _spriteReferences) 
        {
            if (gameObject.name.ToLower().Contains("mainmenubackground"))
            {
                var image = gameObject.GetComponent<Image>();
                image.sprite = _assetController.MainMenuBackgroundSprite;
                image.enabled = true;
                continue;
            }

            var sliderComponent = gameObject.GetComponent<Slider>();
            if (sliderComponent != null)
            {
                Transform background;
                Transform fillArea;

                if (_assetController.UseSliderSprites)
                {
                    sliderComponent.image.sprite = _assetController.SliderKnobSprite;

                    background = sliderComponent.transform.Find("Background");
                    if (background != null)
                    {
                        background.GetComponent<Image>().sprite = _assetController.SliderBackgroundSprite;
                    }

                    fillArea = sliderComponent.transform.Find("Fill Area").transform.Find("Fill");
                    if (fillArea != null)
                    {
                        fillArea.GetComponent<Image>().sprite = _assetController.SliderFillAreaSprite;
                    }
                    continue;
                }

                ColorBlock colors = sliderComponent.colors;
                colors.normalColor = _assetController.NormalUIColor;
                colors.highlightedColor = _assetController.HighlightedUIColor;
                colors.pressedColor = _assetController.PressedUIColor;
                colors.selectedColor = _assetController.SelectedUIColor;
                colors.disabledColor = _assetController.DisabledUIColor;
                sliderComponent.colors = colors;

                fillArea = sliderComponent.transform.Find("Fill Area").transform.Find("Fill");
                fillArea.GetComponent<Image>().color = _assetController.FillUIColor;

                background = sliderComponent.transform.Find("Background");
                background.GetComponent<Image>().color = _assetController.EmptyUIColor;

                continue;
            }

            var buttonComponent = gameObject.GetComponent<Button>();
            if (buttonComponent != null)
            {
                if (_assetController.UseButtonSprite)
                {
                    buttonComponent.image.sprite = _assetController.SliderKnobSprite;
                    continue;
                }

                ColorBlock colors = buttonComponent.colors;
                colors.normalColor = _assetController.NormalUIColor;
                colors.highlightedColor = _assetController.HighlightedUIColor;
                colors.pressedColor = _assetController.PressedUIColor;
                colors.selectedColor = _assetController.SelectedUIColor;
                colors.disabledColor = _assetController.DisabledUIColor;
                buttonComponent.colors = colors;

                Transform text = buttonComponent.transform.Find("Text (TMP)");
                text.GetComponent<TextMeshProUGUI>().font = _assetController.BodyFont;

                continue;
            }

            TextMeshProUGUI textComponent = gameObject.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                if (gameObject.name.ToLower().Contains("title") || gameObject.name.ToLower().Contains("header"))
                {
                   textComponent.font = _assetController.HeaderFont;
                    continue;
                }

                textComponent.font = _assetController.BodyFont;
                continue;
            }
        }
    }
}
