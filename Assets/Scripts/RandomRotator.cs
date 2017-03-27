using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotator : MonoBehaviour {

    private new Rigidbody rigidbody { get { return GetComponent<Rigidbody>(); } }
    public float tumble;

    private void Start()
    {
        rigidbody.angularVelocity = Random.insideUnitSphere * tumble;
    }
}
