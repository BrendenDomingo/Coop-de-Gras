using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "AssetController", menuName = "ScriptableObjects/Asset Controller", order = 1)]
public class AssetController : ScriptableObject
{
    public Sprite MainMenuBackgroundSprite;
    public Sprite ButtonSprite;
    public bool UseButtonSprite;
    public Sprite SliderKnobSprite;
    public Sprite SliderBackgroundSprite;
    public Sprite SliderFillAreaSprite;
    public bool UseSliderSprites;
    public TMP_FontAsset HeaderFont;
    public TMP_FontAsset BodyFont;
    public Color NormalUIColor;
    public Color HighlightedUIColor;
    public Color PressedUIColor;
    public Color SelectedUIColor;
    public Color DisabledUIColor;
    public Color FillUIColor;
    public Color EmptyUIColor;
    public Color HPColor;
    public Color PWRColor;
}
