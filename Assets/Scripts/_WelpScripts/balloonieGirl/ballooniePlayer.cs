using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballooniePlayer : MonoBehaviour
{
    public balloonieGirlManager _balloonieMan;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("called");
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log(" inner called");
            _balloonieMan.badAttempts++;
            
        }
       
    }
}
