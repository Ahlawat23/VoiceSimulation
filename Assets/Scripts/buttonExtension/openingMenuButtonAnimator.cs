using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Serialization;
using System;

public class openingMenuButtonAnimator : Button
{
    //IMPORTANT!!!!!!!


    //PLEASE LET THE BUTTON TEXT BE THE FIRST CHILD
    //THE UNDERLINE IMAGE TO BE SECOND



    private Text ButtonText;
    private Image ButtonImage;

    private GameObject underlineImage;

    private buttonValuesHolder values;

   


    protected override void Awake()
    {
        ButtonText = transform.GetChild(0).GetComponent<Text>();
        ButtonImage = GetComponent<Image>();
        values = GetComponent<buttonValuesHolder>();

        if (values.shouldUnderlineOnHighlight)
        {
            underlineImage = transform.GetChild(1).gameObject;
        }

    }


    public override void OnPointerEnter(PointerEventData eventData)
    {
        transform.LeanScale(values.onHighlightScale, 0.5f).setEaseOutBack();
        image.color = values.onHightlightImageColor;
        ButtonText.color = values.onHighlightTextColor;


        if (values.shouldUnderlineOnHighlight)
        {
            underlineImage.SetActive(true);
        }


    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        transform.LeanScale(new Vector3(1f, 1f, 1f), 0.5f).setEaseOutCirc();
        image.color = values.ImageColor;
        ButtonText.color = values.TextColor;

        if (values.shouldUnderlineOnHighlight)
        {
            underlineImage.SetActive(false);
        }
    }
}
