using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meter : MonoBehaviour
{
    // Start is called before the first frame update
    private Quaternion rotation, localRotation;
    
    void Awake()
    {
        //rotation = transform.rotation;
        localRotation = transform.localRotation; 
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //transform.rotation = rotation;
        transform.localRotation = localRotation;
    }
}
