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
    public GameObject log;

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

    private void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, 0.3f, transform.position.z);
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
        float targetSpeed = speed * Mathf.Clamp(verticalInput + 1.2f, 0, 1.5f);
        _currentSpeed = Mathf.Lerp(_currentSpeed, targetSpeed, Time.deltaTime * 0.5f);
        log.transform.localRotation = Quaternion.Euler(_currentSpeed * 10f - 30f, 180, 0);
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
        print("Game Over");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Gameover();
        }
    }
}