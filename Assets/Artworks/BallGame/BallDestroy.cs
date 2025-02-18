using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallDestroy : MonoBehaviour
{
    public int num;
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void redBallAnim()
    {
        switch (num)
        {
            case 0:
                anim.SetBool("redBall", true);
                anim.SetBool("redBall2", false);
                anim.SetBool("redBall3", false);
                anim.SetBool("redBall4", false);
                anim.SetBool("redBall5", false);
                anim.SetBool("redBall6", false);
                anim.SetBool("redBall7", false);
                num++;
                break;


            case 1:
                anim.SetBool("redBall", false);
                anim.SetBool("redBall2", true);
                anim.SetBool("redBall3", false);
                anim.SetBool("redBall4", false);
                anim.SetBool("redBall5", false);
                anim.SetBool("redBall6", false);
                anim.SetBool("redBall7", false);
                num++;
                break;

            case 2:
                anim.SetBool("redBall", false);
                anim.SetBool("redBall2", false);
                anim.SetBool("redBall3", true);
                anim.SetBool("redBall4", false);
                anim.SetBool("redBall5", false);
                anim.SetBool("redBall6", false);
                anim.SetBool("redBall7", false);
                num++;
                break;

            case 3:
                anim.SetBool("redBall", false);
                anim.SetBool("redBall2", false);
                anim.SetBool("redBall3", false);
                anim.SetBool("redBall4", true);
                anim.SetBool("redBall5", false);
                anim.SetBool("redBall6", false);
                anim.SetBool("redBall7", false);
                num++;
                break;

            case 4:
                anim.SetBool("redBall", false);
                anim.SetBool("redBall2", false);
                anim.SetBool("redBall3", false);
                anim.SetBool("redBall4", false);
                anim.SetBool("redBall5", true);
                anim.SetBool("redBall6", false);
                anim.SetBool("redBall7", false);
                num++;
                break;

            case 5:
                anim.SetBool("redBall", false);
                anim.SetBool("redBall2", false);
                anim.SetBool("redBall3", false);
                anim.SetBool("redBall4", false);
                anim.SetBool("redBall5", false);
                anim.SetBool("redBall6", true);
                anim.SetBool("redBall7", false);
                num++;
                break;

            case 6:
                anim.SetBool("redBall", false);
                anim.SetBool("redBall2", false);
                anim.SetBool("redBall3", false);
                anim.SetBool("redBall4", false);
                anim.SetBool("redBall5", false);
                anim.SetBool("redBall6", false);
                anim.SetBool("redBall7", true);
                num++;
                break;
        }
    }

    public void greenBallAnim()
    {
        switch (num)
        {
            case 0:
                anim.SetBool("greenBall", true);
                anim.SetBool("greenBall2", false);
                anim.SetBool("greenBall3", false);
                anim.SetBool("greenBall4", false);
                anim.SetBool("greenBall5", false);
                anim.SetBool("greenBall6", false);
                anim.SetBool("greenBall7", false);
                num++;
                break;


            case 1:
                anim.SetBool("greenBall", false);
                anim.SetBool("greenBall2", true);
                anim.SetBool("greenBall3", false);
                anim.SetBool("greenBall4", false);
                anim.SetBool("greenBall5", false);
                anim.SetBool("greenBall6", false);
                anim.SetBool("greenBall7", false);
                num++;
                break;

            case 2:
                anim.SetBool("greenBall", false);
                anim.SetBool("greenBall2", false);
                anim.SetBool("greenBall3", true);
                anim.SetBool("greenBall4", false);
                anim.SetBool("greenBall5", false);
                anim.SetBool("greenBall6", false);
                anim.SetBool("greenBall7", false);
                num++;
                break;

            case 3:
                anim.SetBool("greenBall", false);
                anim.SetBool("greenBall2", false);
                anim.SetBool("greenBall3", false);
                anim.SetBool("greenBall4", true);
                anim.SetBool("greenBall5", false);
                anim.SetBool("greenBall6", false);
                anim.SetBool("greenBall7", false);
                num++;
                break;

            case 4:
                anim.SetBool("greenBall", false);
                anim.SetBool("greenBall2", false);
                anim.SetBool("greenBall3", false);
                anim.SetBool("greenBall4", false);
                anim.SetBool("greenBall5", true);
                anim.SetBool("greenBall6", false);
                anim.SetBool("greenBall7", false);
                num++;
                break;

            case 5:
                anim.SetBool("greenBall", false);
                anim.SetBool("greenBall2", false);
                anim.SetBool("greenBall3", false);
                anim.SetBool("greenBall4", false);
                anim.SetBool("greenBall5", false);
                anim.SetBool("greenBall6", true);
                anim.SetBool("greenBall7", false);
                num++;
                break;

            case 6:
                anim.SetBool("greenBall", false);
                anim.SetBool("greenBall2", false);
                anim.SetBool("greenBall3", false);
                anim.SetBool("greenBall4", false);
                anim.SetBool("greenBall5", false);
                anim.SetBool("greenBall6", false);
                anim.SetBool("greenBall7", true);
                num++;
                break;
        }
    }

    public void orangeBallAnim()
    {
        switch (num)
        {
            case 0:
                anim.SetBool("orangeBall", true);
                anim.SetBool("orangeBall2", false);
                anim.SetBool("orangeBall3", false);
                anim.SetBool("orangeBall4", false);
                anim.SetBool("orangeBall5", false);
                anim.SetBool("orangeBall6", false);
                anim.SetBool("orangeBall7", false);
                num++;
                break;


            case 1:
                anim.SetBool("orangeBall", false);
                anim.SetBool("orangeBall2", true);
                anim.SetBool("orangeBall3", false);
                anim.SetBool("orangeBall4", false);
                anim.SetBool("orangeBall5", false);
                anim.SetBool("orangeBall6", false);
                anim.SetBool("orangeBall7", false);
                num++;
                break;

            case 2:
                anim.SetBool("orangeBall", false);
                anim.SetBool("orangeBall2", false);
                anim.SetBool("orangeBall3", true);
                anim.SetBool("orangeBall4", false);
                anim.SetBool("orangeBall5", false);
                anim.SetBool("orangeBall6", false);
                anim.SetBool("orangeBall7", false);
                num++;
                break;

            case 3:
                anim.SetBool("orangeBall", false);
                anim.SetBool("orangeBall2", false);
                anim.SetBool("orangeBall3", false);
                anim.SetBool("orangeBall4", true);
                anim.SetBool("orangeBall5", false);
                anim.SetBool("orangeBall6", false);
                anim.SetBool("orangeBall7", false);
                num++;
                break;

            case 4:
                anim.SetBool("orangeBall", false);
                anim.SetBool("orangeBall2", false);
                anim.SetBool("orangeBall3", false);
                anim.SetBool("orangeBall4", false);
                anim.SetBool("orangeBall5", true);
                anim.SetBool("orangeBall6", false);
                anim.SetBool("orangeBall7", false);
                num++;
                break;

            case 5:
                anim.SetBool("orangeBall", false);
                anim.SetBool("orangeBall2", false);
                anim.SetBool("orangeBall3", false);
                anim.SetBool("orangeBall4", false);
                anim.SetBool("orangeBall5", false);
                anim.SetBool("orangeBall6", true);
                anim.SetBool("orangeBall7", false);
                num++;
                break;

            case 6:
                anim.SetBool("orangeBall", false);
                anim.SetBool("orangeBall2", false);
                anim.SetBool("orangeBall3", false);
                anim.SetBool("orangeBall4", false);
                anim.SetBool("orangeBall5", false);
                anim.SetBool("orangeBall6", false);
                anim.SetBool("orangeBall7", true);
                num++;
                break;

        }
    }

    public void yellowBall()
    {
        anim.SetBool("yellowBall", true);
    }
}
