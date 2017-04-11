using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

    private new Rigidbody rigidbody { get { return GetComponent<Rigidbody>(); } }
    public float speed;

    void Start()
    {
        rigidbody.velocity = transform.forward * speed;
    } 
}
