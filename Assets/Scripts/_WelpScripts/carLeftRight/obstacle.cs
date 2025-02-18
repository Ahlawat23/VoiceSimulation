using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class obstacle : MonoBehaviour
{
    public Transform finalPostion;
    public float timeToReachFinalPos = 3f;

    // Update is called once per frame
    void Update()
    {
        onReachingFinalPos();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            carVoiceRec.instance.onColistion();
        }
    }



    public void moveObstacle()
    {
        transform.LeanMove(finalPostion.position, timeToReachFinalPos);

    }

    void onReachingFinalPos()
    {
        if (transform.position == finalPostion.position)
        {
            Destroy(this.gameObject);
            carVoiceRec.instance.toatalNumOfObs++;
        }

    }
}
