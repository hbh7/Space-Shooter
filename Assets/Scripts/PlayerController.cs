using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{

    private new Rigidbody rigidbody { get { return GetComponent<Rigidbody>(); } }
    private AudioSource audioSource { get { return GetComponent<AudioSource>(); } }
    public float speed;
    public float tilt;
    public Boundary boundary;

    public GameObject shot;
    public Transform shotSpawn;
    public float fireRate;
    private float nextFire;

    private Quaternion calibrationQuaternion;

    private void Start()
    {
        CalibrateAccelerometer();
    } 


    private void Update()
    {
        if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            audioSource.Play();
        }
        if (Input.GetKey("space") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            audioSource.Play();
        }
    }

    void CalibrateAccelerometer ()
    {
        Vector3 accelerationSnapshot = Input.acceleration;
        Quaternion rotationQuaternion = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, -1.0f), accelerationSnapshot);
        calibrationQuaternion = Quaternion.Inverse(rotationQuaternion);
    }

    Vector3 FixAcceleration (Vector3 acceleration)
    {
        Vector3 fixedAcceleration = calibrationQuaternion * acceleration;
        return fixedAcceleration;
    }

    private void FixedUpdate()
    {
        Vector3 movement;
        Vector3 accelerationRaw;
        Vector3 acceleration;
        float moveHorizontal;
        float moveVertical;

#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isRemoteConnected)
        {
            accelerationRaw = Input.acceleration;
            acceleration = FixAcceleration(accelerationRaw);
            movement = new Vector3(acceleration.x, 0.0f, acceleration.y);
            //Debug.Log("REMOTE CONNECTED");
        }
        else
        {
            moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");
            movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
            //Debug.Log("Remote not connected");
            //Debug.Log(movement);
        }
#elif UNITY_ANDROID
        accelerationRaw = Input.acceleration;
        acceleration = FixAcceleration(accelerationRaw);
        movement = new Vector3(acceleration.x, 0.0f, acceleration.y);
        //Debug.Log("Android");
#elif UNITY_STANDALONE
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        //Debug.Log("Standalone");
#elif UNITY_WEBGL
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");
        movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        //Debug.Log("WebGL");
#endif

        //Debug.Log(movement);
        rigidbody.velocity = movement * speed;

        rigidbody.position = new Vector3
            (
                Mathf.Clamp(rigidbody.position.x, boundary.xMin, boundary.xMax),
                0.0f,
                Mathf.Clamp(rigidbody.position.z, boundary.zMin, boundary.zMax)
            );

        rigidbody.rotation = Quaternion.Euler(0.0f, 0.0f, rigidbody.velocity.x * -tilt);
    }
}