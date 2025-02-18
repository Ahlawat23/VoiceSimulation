using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class strawScript : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();    
    }

    // Update is called once per frame
    public void straw()
    {
        anim.SetBool("strawMove", true);
    }

    public void bubble1()
    {
        anim.SetBool("bigBubble", true);
    }

    public void bubble2()
    {
        anim.SetBool("smallBubble", true);
    }
}
