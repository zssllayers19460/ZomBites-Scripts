using UnityEngine;

public class MotoController : MonoBehaviour
{
    public WheelCollider frontWheelCollider;
    public WheelCollider backWheelCollider;
    public Transform frontWheelTransform;
    public Transform backWheelTransform;

    public float maxSteerAngle = 45f;
    public float maxMotorTorque = 200f;
    public float maxBrakeTorque = 400f;

    public bool freezeZAxisRotation = true;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float motorTorque = Input.GetAxis("Vertical") * maxMotorTorque;
        float steerAngle = Input.GetAxis("Horizontal") * maxSteerAngle;

        ApplySteer(steerAngle);
        ApplyTorque(motorTorque);
        ApplyBrake();

        UpdateWheelPoses();
    }

    private void ApplySteer(float steerAngle)
    {
        frontWheelCollider.steerAngle = steerAngle;
    }

    private void ApplyTorque(float motorTorque)
    {
        backWheelCollider.motorTorque = motorTorque;
    }

    private void ApplyBrake()
    {
        float brakeTorque = Input.GetKey(KeyCode.Space) ? maxBrakeTorque : 0f;
        frontWheelCollider.brakeTorque = brakeTorque;
        backWheelCollider.brakeTorque = brakeTorque;
    }

    private void UpdateWheelPoses()
    {
        UpdateWheelPose(frontWheelCollider, frontWheelTransform);
        UpdateWheelPose(backWheelCollider, backWheelTransform);
    }

    private void UpdateWheelPose(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 position;
        Quaternion rotation;
        wheelCollider.GetWorldPose(out position, out rotation);
        wheelTransform.position = position;
        wheelTransform.rotation = rotation;
    }
}