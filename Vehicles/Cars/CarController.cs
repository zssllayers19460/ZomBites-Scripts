using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CarController : MonoBehaviour
{
    public WheelCollider frontLeftCollider, frontRightCollider, rearLeftCollider, rearRightCollider;
    public Transform frontLeftTransform, frontRightTransform, rearLeftTransform, rearRightTransform;
    public GameObject centerOfMass;

    public TextMeshProUGUI speedText;
    public float acceleration = 500f, brakingForce = 500f, maxSpeed = 150f, maxReverseSpeed = 10f, maxSteerAngle = 15f, motorTorqueMultiplier = 5f, reverseTorque = 300f, drag = 0.08f, mass = 750f;
    public bool isAccelerating, isBraking, isTurningLeft, isTurningRight, isReversing;
    

    private float horizontalInput, verticalInput, steeringAngle, currentSpeed;
    private Rigidbody rb;
    private Vector3 vectorMeshPos1, vectorMeshPos2, vectorMeshPos3, vectorMeshPos4;
	private Quaternion quatMesh1, quatMesh2, quatMesh3, quatMesh4;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass.transform.localPosition;
    }

    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelMeshes();
    }

    #region UpdateWheelMesh
    void UpdateWheelMeshes()
    {
        frontRightCollider.GetWorldPose(out vectorMeshPos1, out quatMesh1);
        frontRightTransform.position = vectorMeshPos1;
        frontRightTransform.rotation = quatMesh1;

        frontLeftCollider.GetWorldPose(out vectorMeshPos2, out quatMesh2);
        frontLeftTransform.position = vectorMeshPos2;
        frontLeftTransform.rotation = quatMesh2;

        rearRightCollider.GetWorldPose(out vectorMeshPos3, out quatMesh3);
        rearRightTransform.position = vectorMeshPos3;
        rearRightTransform.rotation = quatMesh3;

        rearLeftCollider.GetWorldPose(out vectorMeshPos4, out quatMesh4);
        rearLeftTransform.position = vectorMeshPos4;
        rearLeftTransform.rotation = quatMesh4;
    }
    #endregion

    private void Accelerate()
    {
        float currentAcceleration = 0f;
        float currentBrakingForce = 0f;

        if (isAccelerating)
        {
            currentAcceleration = acceleration * verticalInput;
        }
        else if (isBraking && !isReversing)
        {
            currentBrakingForce = brakingForce;
        }
        else if (isReversing)
        {
            currentAcceleration = acceleration * verticalInput;
        }

        // Apply motor torque and brake torque to all wheel colliders
        frontLeftCollider.motorTorque = currentAcceleration * motorTorqueMultiplier;
        frontRightCollider.motorTorque = currentAcceleration * motorTorqueMultiplier;
        rearLeftCollider.motorTorque = currentAcceleration * motorTorqueMultiplier;
        rearRightCollider.motorTorque = currentAcceleration * motorTorqueMultiplier;

        frontLeftCollider.brakeTorque = currentBrakingForce * motorTorqueMultiplier;
        frontRightCollider.brakeTorque = currentBrakingForce * motorTorqueMultiplier;
        rearLeftCollider.brakeTorque = currentBrakingForce * motorTorqueMultiplier;
        rearRightCollider.brakeTorque = currentBrakingForce * motorTorqueMultiplier;

        // Apply drag
        rb.drag = drag;
        rb.mass = mass;

        // Cap the current speed if it exceeds the maximum speed
        if (currentSpeed >= maxSpeed && !isReversing)
        {
            float currentSpeedInDirection = Vector3.Dot(rb.velocity, transform.forward);
            rb.AddForce(-transform.forward * (currentSpeedInDirection - maxSpeed), ForceMode.Acceleration);
        }
        else if (currentSpeed >= maxReverseSpeed && isReversing)
        {
            float currentSpeedInDirection = Vector3.Dot(rb.velocity, -transform.forward);
            rb.AddForce(transform.forward * (currentSpeedInDirection - maxReverseSpeed), ForceMode.Acceleration);
        }

        currentSpeed = rb.velocity.magnitude;

        speedText.text = Mathf.Round(currentSpeed * 2.237f) + " MPH";
    }

    private void Steer()
    {
        float normalizedHorizontalInput = Mathf.Clamp(horizontalInput, -1f, 1f);
        float steeringAngle = maxSteerAngle * normalizedHorizontalInput;

        frontLeftCollider.steerAngle = steeringAngle;
        frontRightCollider.steerAngle = steeringAngle;

        // Adjust wheel visual rotation for better steering visualization
        float wheelRotationAngle = steeringAngle / maxSteerAngle * 30f;
        frontLeftTransform.localEulerAngles = new Vector3(frontLeftTransform.localEulerAngles.x, wheelRotationAngle, frontLeftTransform.localEulerAngles.z);
        frontRightTransform.localEulerAngles = new Vector3(frontRightTransform.localEulerAngles.x, wheelRotationAngle, frontRightTransform.localEulerAngles.z);

        // Determine if the car is turning left or right
        isTurningLeft = steeringAngle < 0f;
        isTurningRight = steeringAngle > 0f;
    }

    public void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        isAccelerating = verticalInput > 0f;
        isBraking = Input.GetKey(KeyCode.Space);
        isReversing = verticalInput < 0f || Input.GetKey(KeyCode.S);
    }
}