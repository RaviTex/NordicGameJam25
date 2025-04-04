using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float steeringForce = 5f;
    [SerializeField] private float driftTime = 0.5f;

    private Rigidbody _rb;

    private float _yRotation;
    private Vector3 _shipForward;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _shipForward = transform.forward;
    }

    private void Update()
    {
        _shipForward = Vector3.Lerp(_shipForward, transform.forward, Time.deltaTime * driftTime);
        _shipForward.Normalize();
        Debug.DrawLine(transform.position, transform.position + _shipForward * 5f, Color.red);
    }

    private void FixedUpdate()
    {
        _rb.velocity = _shipForward * (speed * Mathf.Clamp01(Input.GetAxis("Vertical") + 1));

        _yRotation += Input.GetAxis("Horizontal") * steeringForce;
        transform.rotation = Quaternion.Euler(0, _yRotation, 0);
    }
}