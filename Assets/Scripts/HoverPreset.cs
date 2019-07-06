using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Anchor and positioning data for hover details.
/// </summary>
[CreateAssetMenu(fileName = "new_HoverPreset", menuName = "Hover Preset")]
public class HoverPreset : ScriptableObject
{
    public bool ignoreLayout = true;
    public bool disableOnExit = true;
    [Header("Appearance")]
    public Color colour = Color.white;
    public Sprite sprite;
    public Image.Type imageType = Image.Type.Sliced;

    [Header("Content Fit")]
    public ContentSizeFitter.FitMode horizontalFit;
    public ContentSizeFitter.FitMode verticalFit;
    public float spacing;

    [Header("Positioning")]
    public Vector2 anchorMin;
    public Vector2 anchorMax;
    public Vector2 offset;
}
