using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InputFieldExtention : InputField
{
    private Transform SelectBgImage;

    protected override void Awake()
    {
        SelectBgImage = transform.GetChild(0);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
       
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        
        

        EventSystem.current.SetSelectedGameObject(gameObject, eventData);

        

        UpdateLabel();
        eventData.Use();
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
       
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        

        base.OnDeselect(eventData);
    }

    
}
