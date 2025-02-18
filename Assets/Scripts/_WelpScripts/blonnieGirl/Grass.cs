using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grass : MonoBehaviour
{

    public Transform finalPostion;
    public float timeToReachFinalPos = 3f;
    public float grassheight;

    // Update is called once per frame
    void Update()
    {
        onReachingFinalPos();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            bloonieGirlManager.instance.GameOver();
        }
    }

    

    public  void MoveGrassToPlayer()
    {
        if(grassheight != 0)
        transform.localScale = new Vector3(0.2f, grassheight);
        transform.LeanMove(finalPostion.position, timeToReachFinalPos);
        
    }
  
    void onReachingFinalPos()
    {
        if (transform.position == finalPostion.position)
        {
            Destroy(this.gameObject);
            bloonieGirlManager.instance.numOfHurdlesCrossed++;
            if (bloonieGirlManager.instance.numOfHurdlesCrossed % 4 == 0)
            {
                bloonieGirlManager.instance.level++; 
                
                bloonieGirlManager.instance.showLevel(bloonieGirlManager.instance.level);
            }
        }
            
    }
}
