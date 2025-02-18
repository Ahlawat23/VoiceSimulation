using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSCripts : MonoBehaviour
{
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void planetEarth()
    {
        anim.SetBool("EARTH", true);
    }
    public void planetMercury()
    {
        anim.SetBool("MERCURY", true);
    }

    public void planetJupiter()
    {
        anim.SetBool("JUPITER", true);
    }

    public void planetSaturn()
    {
        anim.SetBool("SATURN", true);
    }
}
