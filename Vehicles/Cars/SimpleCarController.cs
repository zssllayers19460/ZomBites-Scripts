using System.Collections.Generic;
using UnityEngine;

public class SimpleCarController : MonoBehaviour
{
    public WheelCollider frontLeftCollider, frontRightCollider, rearLeftCollider, rearRightCollider;
    public Transform frontLeftTransform, frontRightTransform, rearLeftTransform, rearRightTransform;

    public float acceleration = 500f, brakingForce = 500f, maxSpeed = 150f, maxReverseSpeed = 10f, maxSteerAngle = 15f, motorTorqueMultiplier = 5f, reverseTorque = 300f, drag = 0.08f, mass = 750f;
    public bool isAccelerating, isBraking, isTurningLeft, isTurningRight, isReversing;

    private float horizontalInput, verticalInput, steeringAngle, currentSpeed;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelTransforms();
    }

    private void UpdateWheelTransforms()
{
    UpdateWheelTransform(frontLeftCollider, frontLeftTransform);
    UpdateWheelTransform(frontRightCollider, frontRightTransform);
    UpdateWheelTransform(rearLeftCollider, rearLeftTransform);
    UpdateWheelTransform(rearRightCollider, rearRightTransform);
}

private void UpdateWheelTransform(WheelCollider wheelCollider, Transform wheelTransform)
{
    Vector3 position;
    Quaternion rotation;
    wheelCollider.GetWorldPose(out position, out rotation);
    wheelTransform.position = position;
    wheelTransform.rotation = rotation;
}

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