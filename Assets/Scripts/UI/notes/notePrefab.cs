using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class notePrefab : MonoBehaviour
{
    public RectTransform thisTransform;
    public Text noteText;
    public Text DateText;
    public int textLength;

    public float minHieght = 150;
    public float hieghtForEveryline = 100;
    public int charcterInOnelIne = 55;

    public void setText(string text, string date)
    {
        noteText.text = text;
        DateText.text = date;
        textLength = noteText.text.Length;
        float height = ((textLength / charcterInOnelIne) * hieghtForEveryline) + minHieght;
        Debug.Log((textLength / charcterInOnelIne));
        Debug.Log(height);
        thisTransform.sizeDelta = new Vector2(thisTransform.sizeDelta.x, height);
    }

   
    
}
