using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class dragWindow : MonoBehaviour, IDragHandler
{
    public RectTransform dragRectform;
    public void OnDrag(PointerEventData eventData)
    {
        dragRectform.anchoredPosition += eventData.delta;
    }

   
}
