using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class candleGlow : MonoBehaviour
{
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void candle1()
    {
        anim.SetBool("oraCandle", true);
    }

    public void candle2()
    {
        anim.SetBool("purCandle", true);
    }

    public void candle3()
    {
        anim.SetBool("greeCandle", true);
    }
}
