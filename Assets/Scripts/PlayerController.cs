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
    [SerializeField] private float customYLevel;

    public int wood;
    private Rigidbody _rb;
    private float _yRotation;
    private Vector3 _shipForward;

    private float _currentSpeed;

    // public GameObject targetGameObject;
    public GameObject log;

    private Quaternion _startRotation;

    public ParticleSystem logSplash;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _shipForward = transform.forward;
        _startRotation = transform.rotation;
    }

    private void Update()
    {
        UpdateShipForward();
        DebugDrawShipForward();
        _rb.MovePosition(new Vector3(transform.position.x, customYLevel, transform.position.z));
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
        Quaternion targetRotation = Quaternion.Euler(_currentSpeed * 10f - 35f, 180, 0);
        if (verticalInput == 0)
        {
            targetRotation = Quaternion.Euler(0, 180, 0);
        }

        log.transform.localRotation = Quaternion.Lerp(log.transform.localRotation, targetRotation, Time.deltaTime * 0.5f);
        _rb.velocity = _shipForward * _currentSpeed;
    }

    private void HandleSteering()
    {
        float calculatedSteeringForce = Mathf.Lerp(1, steeringForce, _currentSpeed / speed);
        _yRotation += Input.GetAxis("Horizontal") * calculatedSteeringForce;
        transform.rotation = Quaternion.Euler(0, _yRotation, 0) * _startRotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("DropOff"))
        {
            if (wood > 0)
            {
                wood = 0;
                // targetGameObject.SetActive(false);
                GameManager.Instance.curZonePairActive++;
                other.GetComponentInParent<InteractionBuildings>().StartDisable();
            }
        }
        else if (other.gameObject.CompareTag("PickUp"))
        {
            wood++;
            // targetGameObject.SetActive(true);
            GameManager.Instance.DisableMarker(false);
            other.GetComponentInParent<InteractionBuildings>().StartDisable();
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