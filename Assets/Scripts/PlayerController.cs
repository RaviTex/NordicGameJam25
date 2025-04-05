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

    public int wood;
    private Rigidbody _rb;
    private float _yRotation;
    private Vector3 _shipForward;
    private float _currentSpeed;
    public GameObject targetGameObject;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _shipForward = transform.forward;
    }

    private void Update()
    {

        UpdateShipForward();
        DebugDrawShipForward();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleSteering();
    }

    private void UpdateShipForward()
    {
        _shipForward = Vector3.Lerp(_shipForward, transform.forward, Time.deltaTime * driftTime);
        _shipForward.Normalize();
    }

    private void DebugDrawShipForward()
    {
        Debug.DrawLine(transform.position, transform.position + _shipForward * 5f, Color.red);
    }

    private void HandleMovement()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float targetSpeed = speed * Mathf.Clamp01(verticalInput + 1.2f);
        _currentSpeed = Mathf.Lerp(_currentSpeed, targetSpeed, Time.deltaTime * 0.5f);
        _rb.velocity = _shipForward * _currentSpeed;
    }

    private void HandleSteering()
    {
        float calculatedSteeringForce = Mathf.Lerp(0, steeringForce, _currentSpeed / speed);
        _yRotation += Input.GetAxis("Horizontal") * calculatedSteeringForce;
        transform.rotation = Quaternion.Euler(0, _yRotation, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DropOff"))
        {
            wood = 0;
            targetGameObject.SetActive(false);
            Debug.Log("hit");

        }
        else if (other.gameObject.CompareTag("PickUp"))
        {
            wood++;
            targetGameObject.SetActive(true);


        }
    }
    private void Gameover()
    {
        Gameover();   
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
            {
            Gameover();
        }
    }
    
}
    