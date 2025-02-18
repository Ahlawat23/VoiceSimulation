using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rabbitscript : MonoBehaviour
{

    Animator anim;

    public int count;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    //public void jumpingHorse()
    //{
    //    anim.Play("horseRun");
    //}

    public void jumpRabbit()
    {
        switch (count)
        {
            case 0:
                anim.SetBool("moveRabbit", true);
                anim.SetBool("moveRabbit2", false);
                anim.SetBool("moveRabbit3", false);
                anim.SetBool("moveRabbit4", false);
                count++;
                break;

            case 1:
                anim.SetBool("moveRabbit", false);
                anim.SetBool("moveRabbit2", true);
                anim.SetBool("moveRabbit3", false);
                anim.SetBool("moveRabbit4", false);
                count++;
                break;

            case 2:
                anim.SetBool("moveRabbit", false);
                anim.SetBool("moveRabbit2", false);
                anim.SetBool("moveRabbit3", true);
                anim.SetBool("moveRabbit4", false);
                count++;
                break;

            case 3:
                anim.SetBool("moveRabbit", false);
                anim.SetBool("moveRabbit2", false);
                anim.SetBool("moveRabbit3", false);
                anim.SetBool("moveRabbit4", true);
                count++;
                break;
        }


    }

    public void switchRabbit()
    {
        switch (count)
        {
            case 0:
                anim.SetBool("rabbitLegs", true);
                anim.SetBool("rabbitLegs2", false);
                anim.SetBool("rabbitLges3", false);
                count++;
                break;

            case 1:
                anim.SetBool("rabbitLegs", false);
                anim.SetBool("rabbitLegs2", true);
                anim.SetBool("rabbitLges3", false);
                count++;
                break;

            case 2:
                anim.SetBool("rabbitLegs", false);
                anim.SetBool("rabbitLegs2", false);
                anim.SetBool("rabbitLges3", true);
                count++;
                break;
        }
    }
}

