using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketAnim : MonoBehaviour
{
    Animator anim;
    public int count;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void Basket()
    {
        switch (count)
        {
            case 0:
                anim.SetBool("Basket1", true);
                anim.SetBool("Basket2", false);
                anim.SetBool("Basket3", false);
                anim.SetBool("Basket4", false);
                count++;
                break;

            case 1:
                anim.SetBool("Basket1", false);
                anim.SetBool("Basket2", true);
                anim.SetBool("Basket3", false);
                anim.SetBool("Basket4", false);
                count++;
                break;

            case 2:
                anim.SetBool("Basket1", false);
                anim.SetBool("Basket2", false);
                anim.SetBool("Basket3", true);
                anim.SetBool("Basket4", false);
                count++;
                break;

            case 3:
                anim.SetBool("Basket1", false);
                anim.SetBool("Basket2", false);
                anim.SetBool("Basket3", false);
                anim.SetBool("Basket4", true);
                count++;
                break;

        }
    }
}
