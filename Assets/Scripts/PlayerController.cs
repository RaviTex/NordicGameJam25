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

    [SerializeField] private GameObject feedBackLogs;
    public GameObject log;

    private Quaternion _startRotation;

    public ParticleSystem logSplash;

    public bool isInputEnabled = true;

    private EngineState _engineState;
    private EngineState _oldEngineState;

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

        float verticalInput = Input.GetAxis("Vertical");
        if (verticalInput > 0f && _engineState != EngineState.Fast)
        {
            _engineState = EngineState.Fast;
            AudioManager.instance.EngineFast();
        }
        else if (verticalInput < 0f && _engineState != EngineState.Idle)
        {
            _engineState = EngineState.Idle;
            AudioManager.instance.EngineIdle();
        }
        else if (verticalInput == 0f && _engineState != EngineState.Slow)
        {
            _engineState = EngineState.Slow;
            AudioManager.instance.EngineSlow();
        }
    }

    private void FixedUpdate()
    {
        if (!isInputEnabled) return;
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
        _currentSpeed = Mathf.Lerp(_currentSpeed, targetSpeed, Time.deltaTime * 0.6f);
        Quaternion targetRotation = Quaternion.Euler(Mathf.Clamp(_currentSpeed * 10f - 35f, -15f, 30), 180, 0);
        if (verticalInput == 0)
        {
            targetRotation = Quaternion.Euler(0, 180, 0);
        }

        log.transform.localRotation = Quaternion.Lerp(log.transform.localRotation, targetRotation, Time.deltaTime * 2f);
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
                feedBackLogs.SetActive(false);
                GameManager.Instance.curZonePairActive++;
                UIManager.Instance.UpdateDeliveryBar();
                other.GetComponentInParent<InteractionBuildings>().StartDisable();
            }
        }
        else if (other.gameObject.CompareTag("PickUp"))
        {
            wood++;
            feedBackLogs.SetActive(true);
            GameManager.Instance.DisableMarker(false);
            other.GetComponentInParent<InteractionBuildings>().StartDisable();
        }
    }
}

public enum EngineState
{
    Idle,
    Slow,
    Fast
}