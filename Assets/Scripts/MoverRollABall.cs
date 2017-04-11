using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoverRollABall : MonoBehaviour {

    private new Rigidbody rigidbody { get { return GetComponent<Rigidbody>(); } }
    public float speed;

    void Start()
    {
        int[] Angles = new int[2] { Random.Range(-60, -40), Random.Range(40, 60) };
        int RandomIndex = Random.Range(0, 2);

        Debug.Log(Angles);
        Debug.Log(RandomIndex);
        Debug.Log(Angles[RandomIndex]);


        transform.Rotate(0, Angles[RandomIndex], 0);


        rigidbody.velocity = transform.forward * speed;

    } 
}
