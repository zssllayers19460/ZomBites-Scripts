using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FreeFlyCamera : MonoBehaviour
{
    [SerializeField] private KeyCode flyKey = KeyCode.N;
    [SerializeField] private KeyCode boostSpeed = KeyCode.LeftShift;
    [SerializeField] private float mouseSense = 1.8f;
    [SerializeField] private float movementSpeed = 10f;

    [SerializeField] private bool flying = false;

    private void Update()
    {
        if (Input.GetKeyDown(flyKey))
        {
            flying = !flying;
            Cursor.lockState = flying ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !flying;

            if (!flying)
            {
                Input.ResetInputAxes();
                return;
            }
        }

        if (!flying || Cursor.visible)
            return;

        float currentSpeed = Input.GetKey(boostSpeed) ? movementSpeed * 2f : movementSpeed;

        Vector3 forwardDirection = transform.forward;
        forwardDirection.y = 0; // Ignore vertical component
        forwardDirection.Normalize();

        Vector3 rightDirection = transform.right;
        rightDirection.y = 0; // Ignore vertical component
        rightDirection.Normalize();

        Vector3 deltaPosition = forwardDirection * Input.GetAxis("Vertical") + rightDirection * Input.GetAxis("Horizontal");
        deltaPosition.Normalize();

        transform.position += deltaPosition * currentSpeed * Time.deltaTime;

        float mouseY = -Input.GetAxis("Mouse Y") * mouseSense;
        float mouseX = Input.GetAxis("Mouse X") * mouseSense;

        // Limit the vertical rotation to avoid flipping the camera
        float currentXRotation = transform.eulerAngles.x;
        float desiredXRotation = currentXRotation + mouseY;
        float clampedXRotation = Mathf.Clamp(desiredXRotation, 0f, 90f);

        transform.rotation *= Quaternion.Euler(clampedXRotation, mouseX, 0f);
    }
}