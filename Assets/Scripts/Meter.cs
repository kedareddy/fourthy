using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meter : MonoBehaviour
{
    // Start is called before the first frame update
    private Quaternion rotation;
    private Vector3 scale;
    private Transform parent; 
    void Awake()
    {
        rotation = transform.rotation;
        parent = transform.parent; 
        scale = transform.lossyScale; 
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = rotation;
        //transform.SetParent(null);
        //transform.localScale = scale;
        //transform.SetParent(parent);
    }
}
