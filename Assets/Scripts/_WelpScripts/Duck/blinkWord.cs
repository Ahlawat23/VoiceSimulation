using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class blinkWord : MonoBehaviour
{
    public Text[] keywordText;
    public bool[] shouldBlink;
    List<Coroutine> activeCoroutine = new List<Coroutine>();
    public float secsToWaitInBetweenShift = 0.2f;
    public Vector3 maxSize;
    public Vector3 minSize;
    public float timeForExpand;

   


    public void switchText(int index)
    {
        switch (index)
        {
            case 0:
                blnkTextOfIndex(0);
                break;
            case 1:

                blnkTextOfIndex(1);
                break;
            case 2:
                blnkTextOfIndex(2);
                break;
            case 3:
                blnkTextOfIndex(3);
                break;
            case 4:
                blnkTextOfIndex(4);
                break;
            case 5:
                blnkTextOfIndex(5);
                break;
        }
    }



    public void blnkTextOfIndex(int index)
    {
        
        clearAllCoroutine();
        setAllTextWhite();
        setBoolBlink(index);
        activeCoroutine.Add(StartCoroutine(blinktext_coroutine(index)));
        activeCoroutine.Add(StartCoroutine(expantContract_coroutine(index)));
    }


    void setBoolBlink(int index)
    {
        for (int i = 0; i < shouldBlink.Length; i++)
        {
            if (i == index)
                shouldBlink[i] = true;
            else
                shouldBlink[i] = false;
        }
        
    }

    public void clearAllCoroutine()
    {
        for (int i = 0; i < activeCoroutine.Count; i++)
        {
            StopCoroutine(activeCoroutine[i]);
        }

        activeCoroutine.Clear();
    }
    public void setAllTextWhite()
    {
        for (int i = 0; i < keywordText.Length; i++)
        {
            keywordText[i].color = Color.white;
        }
    }
    IEnumerator blinktext_coroutine(int index)
    {
        
        keywordText[index].color = Color.red;
        yield return new WaitForSeconds(secsToWaitInBetweenShift);
        keywordText[index].color = Color.cyan;
        yield return new WaitForSeconds(secsToWaitInBetweenShift);
        keywordText[index].color = Color.green;
        yield return new WaitForSeconds(secsToWaitInBetweenShift);

        if (shouldBlink[index])
            activeCoroutine.Add(StartCoroutine(blinktext_coroutine(index)));
        
    }

    IEnumerator expantContract_coroutine(int index)
    {
        keywordText[index].transform.LeanScale(maxSize, timeForExpand).setEaseInOutBack();
        yield return new WaitForSeconds(timeForExpand);
        keywordText[index].transform.LeanScale(minSize, timeForExpand).setEaseInOutBack();
        yield return new WaitForSeconds(timeForExpand);
        if (shouldBlink[index])
            activeCoroutine.Add(StartCoroutine(expantContract_coroutine(index)));
    }

    
}
