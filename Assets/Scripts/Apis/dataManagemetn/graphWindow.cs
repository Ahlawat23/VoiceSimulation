using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class graphWindow : MonoBehaviour
{
    public Sprite circleSprite;
    public RectTransform graphcontainer;
    public float yMax = 100f;
    public float xSiz = 50f;

    public RectTransform origin;

    private void Awake()
    {

      
    }
    public void CalculateGraph(List<int> valueList)
    {
        float graphHeight = graphcontainer.sizeDelta.y;
        float yMaximum = yMax;
        float xSize = getXsize(valueList.Count);
        //Debug.Log("for count : " + valueList.Count + "xsize is : " + xSize);

        

        GameObject lastCircleGameObject = null;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPostion = xSize + i * xSize;
            
            
            float YPostion = (valueList[i] / yMaximum) * graphHeight;
           
            GameObject circleGameObject = createCircle(new Vector2(xPostion, YPostion));
            if (lastCircleGameObject != null)
            {
                createDotConnection(lastCircleGameObject.GetComponent<RectTransform>().anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            else
            {
               
                createDotConnection(origin.anchoredPosition, circleGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            lastCircleGameObject = circleGameObject;
        }
    }

    GameObject createCircle(Vector2 anchoredPostion)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphcontainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        gameObject.GetComponent<Image>().color = new Color(0, 0.5f, 1, 1 );
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPostion;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMax = Vector2.zero;
        rectTransform.anchorMin = Vector2.zero;
        return gameObject;
    }

    void createDotConnection(Vector2 dotPosA, Vector2 dotPosB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphcontainer, false);
        gameObject.GetComponent<Image>().color = Color.black;
        RectTransform rectTrans = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPosB - dotPosA).normalized;
        float distance = Vector2.Distance(dotPosA, dotPosB);
        rectTrans.anchorMin = new Vector2(0, 0);
        rectTrans.anchorMax = new Vector2(0, 0);
        rectTrans.sizeDelta = new Vector2(distance, 3f);
        rectTrans.anchoredPosition = dotPosA + dir * distance * 0.5f;
        rectTrans.localEulerAngles = new Vector3(0, 0, GetAngleFromVector(dir));
    }

    public int GetAngleFromVector(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        int angle = Mathf.RoundToInt(n);

        return angle;
    }

    float getXsize(int count)
    {
        float xsize = 10;
        if (count < 5)
        {
            xsize = 20;
        }
        if (count > 10)
        {
            xsize = 15;
        }
        if (count > 20)
        {
            xsize = 10;
        }
        if (count > 30)
        {
            xsize = 5;
        }
        if (count > 60)
        {
            xsize = 3;
        }
        if (count > 80)
        {
            xsize = 2;
        }
        

        return xsize;
    }
}
