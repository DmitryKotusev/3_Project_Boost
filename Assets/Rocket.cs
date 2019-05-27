using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField]
    float rcsThrust = 100f;
    [SerializeField]
    float mainThrust = 20f;
    Rigidbody rigidBody;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Thrust();
        Rotate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                {
                    Debug.Log("Ok");
                    break;
                }
            case "Fuel":
                {
                    Debug.Log("Fuel");
                    break;
                }
            default:
                {
                    Debug.Log("Dead");
                    break;
                }
        }
    }

    private void Rotate()
    {
        if (Input.GetKey(KeyCode.A))
        {
            //rigidBody.freezeRotation = true;
            float rotationThisFrame = rcsThrust * Time.deltaTime;
            transform.Rotate(Vector3.forward * rotationThisFrame);
            //rigidBody.freezeRotation = false;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //rigidBody.freezeRotation = true;
            float rotationThisFrame = rcsThrust * Time.deltaTime;
            transform.Rotate(Vector3.forward * (-rotationThisFrame));
            //rigidBody.freezeRotation = false;
        }
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust);
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
}
