using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonValuesHolder : MonoBehaviour
{
    [Header("Image Colors")]
    public Color onHightlightImageColor;
    public Color ImageColor;

    [Header("TextColor")]
    public Color onHighlightTextColor;
    public Color TextColor;

    [Header("Text Manupulation")]
    public bool shouldUnderlineOnHighlight;

    [Header("on highlight scale")]
    public Vector3 onHighlightScale = new Vector3(1.5f, 1.5f, 1.5f);
    

}
