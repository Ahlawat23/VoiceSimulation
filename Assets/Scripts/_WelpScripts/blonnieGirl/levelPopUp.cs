using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;


public class levelPopUp : MonoBehaviour
{
    public UnityEvent onLevelOneStart;
    public UnityEvent onLevelTwoStart;
    public UnityEvent onLevelThreeStart;
    public UnityEvent onLevelFourStart;

    public bool isGameOver = false;
    public bool[] isLevelFinished;


    private void OnEnable()
    {
        
        if(isLevelFinished[0])
        StartCoroutine(level1Start());
        if (isLevelFinished[1])
            StartCoroutine(level2Start());
        if (isLevelFinished[2])
            StartCoroutine(level3Start());
        if (isLevelFinished[3])
            StartCoroutine(level4Start());
    }
    public void resetGame()
    {
        for (int i = 0; i < isLevelFinished.Length; i++)
        {
            isLevelFinished[i] = false;
        }
    }
    IEnumerator level1Start()
    {
        yield return new WaitForSeconds(2);
        if (!isGameOver)
        {
           
            gameObject.SetActive(false);
            onLevelOneStart.Invoke();
        }
            
       
        
        
    }

    IEnumerator level2Start()
    {
       
        if (!isGameOver)
        {
            yield return new WaitForSeconds(2);
            gameObject.SetActive(false);
            onLevelTwoStart.Invoke();
        }


    }
    IEnumerator level3Start()
    {
       
        if (!isGameOver)
        {
            yield return new WaitForSeconds(2);
            gameObject.SetActive(false);
            onLevelThreeStart.Invoke();
        }


    }
    IEnumerator level4Start()
    {
       
        if (!isGameOver)
        {
            yield return new WaitForSeconds(2);
            gameObject.SetActive(false);
            onLevelFourStart.Invoke();
        }


    }






}
