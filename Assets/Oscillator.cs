using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField]
    Vector3 movementVector;

    [SerializeField]
    float period;

    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float cycles = Time.time / period;
        float tau = Mathf.PI * 2;
        Vector3 offset = movementVector * (1 + Mathf.Sin(tau * cycles)) / 2;
        transform.position = startPos + offset;
    }
}
