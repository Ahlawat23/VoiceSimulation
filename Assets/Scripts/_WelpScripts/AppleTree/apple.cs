using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class apple : MonoBehaviour
{
    public Transform finalPos;
    public float timeToDrop;
    public Transform truck;

    private void Start()
    {
        finalPos = transform.GetChild(0).transform;
    }

    public void dropApple()
    {
        transform.LeanMove(finalPos.position, timeToDrop);
        transform.SetParent(truck);
    }
}
