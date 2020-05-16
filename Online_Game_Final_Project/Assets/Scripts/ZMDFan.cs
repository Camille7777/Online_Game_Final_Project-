using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZMDFan : MonoBehaviour
{
    int speed = 40;
    void Start()
    {


    }


    public void Update()
    {
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);

    }
}
