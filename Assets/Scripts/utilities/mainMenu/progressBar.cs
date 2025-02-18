using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class progressBar : MonoBehaviour
{

    public float fillamountIncrement = 0.05f;
    public float seconds = 0.05f;
    public Image fill;
    public float fillamount = 0;
    public Text loadingText;

    private void OnEnable()
    {
        StartCoroutine(iterateFillAmount_corountine());
    }

    private void OnDisable()
    {
        StopCoroutine(iterateFillAmount_corountine());
    }
    private void Update()
    {
       


       fill.fillAmount = fillamount;
    }

    public void setLoadingTest(string _loadingText)
    {
        loadingText.text = _loadingText;
    }

   IEnumerator iterateFillAmount_corountine()
    {
        if (fillamount < 1)
            fillamount = fillamount + fillamountIncrement;
        else
            fillamount = 0;

        yield return new WaitForSeconds(seconds);

        StartCoroutine(iterateFillAmount_corountine());
    }
}
