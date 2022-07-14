using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialRotationOpening : MonoBehaviour
{
    private float rotationSpeed;

    //1 is clockwise, -1 is counter clockwise
    public float direction;
    private void Start()
    {
        rotationSpeed = Random.Range(10, 30)*direction;
    }


    void Update()
    {
        //rotates the dial over time
        transform.Rotate(Vector3.back*Time.deltaTime*rotationSpeed);
    }
}
