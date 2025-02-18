using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class circleLoadingBar : MonoBehaviour
{
   [Range(0, 100)]
   public float fillvalue = 0;
    public Image circleFillImage;
    public RectTransform handlerImage;
    public RectTransform fillHandeler;

    

    // Update is called once per frame
    void Update()
    {
        if(fillvalue != 100)
        fillvalue = fillvalue + 1;
        else
            fillvalue = 0;
        fillcircularValue(fillvalue);
    }
    
    void fillcircularValue(float value)
    {
        float fillAmount = (value / 100.0f);
        circleFillImage.fillAmount = fillAmount;
        float angle = fillAmount * 360;
        fillHandeler.localEulerAngles = new Vector3(0, 0, -angle);
        handlerImage.localEulerAngles = new Vector3(0, 0, angle);
    }
}
