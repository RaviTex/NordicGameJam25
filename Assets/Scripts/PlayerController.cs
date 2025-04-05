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

        // var colliders = Physics.OverlapBox(transform.position, new Vector3(2f, 1f, 2.5f), Quaternion.identity, LayerMask.GetMask("Land"));
        // if(colliders.Length > 0)
        // {
        //     foreach (var collider in colliders)
        //     {
        //         print("Collided with land");
        //         Ray ray = new Ray(transform.position, collider.ClosestPoint(transform.position) - transform.position);
        //         if (Physics.Raycast(ray, out RaycastHit hit))
        //         {
        //             Debug.DrawLine(ray.origin, hit.point, Color.red);
        //             var newShipForward = Vector3.ProjectOnPlane(hit.normal, ).normalized;
        //             Debug.DrawLine(transform.position, transform.position + newShipForward * 5f, Color.green);
        //             transform.forward = newShipForward;
        //         }
        //     }
        // }
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
            
           
        }
        else if (other.gameObject.CompareTag("PickUp"))
        {
            wood++; 
        }
        

        
       
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.DrawCube(transform.position, new Vector3(4f, 2f, 5f));
    // }
}