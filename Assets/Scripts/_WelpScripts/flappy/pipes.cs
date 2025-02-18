using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pipes : MonoBehaviour
{

    public Transform finalPostion;
    public float timeToReachFinalPos = 3f;
    public float grassheight;

    // Update is called once per frame
    void Update()
    {
        if (flappyManager.instance.isgameover)
            Destroy(this.gameObject);
        onReachingFinalPos();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //flappyManager.instance.GameOver();
            flappyManager.instance.badAttempts++;
        }
    }

   


    public void MoveGrassToPlayer()
    {
        if (grassheight != 0)
        {
            transform.GetChild(0).localScale = new Vector3(grassheight, grassheight);
        }
            
        transform.LeanMove(finalPostion.position, timeToReachFinalPos);

    }

    void onReachingFinalPos()
    {
        if (transform.position == finalPostion.position)
        {
            Destroy(this.gameObject);
            if (flappyManager.instance.isgameover)
                return;
            flappyManager.instance.numOfHurdlesCrossed++;
            if (flappyManager.instance.numOfHurdlesCrossed % 4 == 0)
            {
                flappyManager.instance.level++;

                flappyManager.instance.showLevel(flappyManager.instance.level);
            }

        }
    }
}
