using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	#region 
	public static Transform instance;
	
	void Awake()
	{
		LockCursor();
		playerCamera = GetComponentInChildren<Camera>();
		characterController = GetComponent<CharacterController>();
		defaultYPos = playerCamera.transform.localPosition.y;
		audioSource = GetComponent<AudioSource>();
		instance = this.transform;
	}
	// Its up here for the Region
	#endregion
	
	public bool CanMove { get; private set; } = true;
	private AudioSource audioSource;
	private bool IsSprinting => canSprint && Input.GetKey(sprintKey);
	private bool ShouldJump => Input.GetKeyDown(jumpKey) && characterController.isGrounded;
	private bool Shouldcrouch => Input.GetKeyDown(crouchKey) && !duringCrounchAnimation && characterController.isGrounded;

	[Header("Funtional Options")]
	[SerializeField] private bool canSprint = true;
	[SerializeField] private bool canJump = true;
	[SerializeField] private bool canCrouch = true;
	[SerializeField] private bool canFootsteps = true;
	[SerializeField] private bool canUseHeadbob = true;
	[SerializeField] private bool willSlideOnSlopes = true;

	[Header("Controls")]
	[SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
	[SerializeField] private KeyCode jumpKey = KeyCode.Space;
	[SerializeField] private KeyCode crouchKey = KeyCode.C;

	[Header("Movement Parameters")]
	[SerializeField] private float walkSpeed = 3.0f;
	[SerializeField] private float sprintSpeed = 6.0f;
	[SerializeField] private float crouchSpeed = 1.5f;
	[SerializeField] private float slopeSpeed = 8f;

	[Header("Look Parameters")]
	[SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
	[SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
	[SerializeField, Range(1, 180)] private float upperLookLimit = 80.0f;
	[SerializeField, Range(1, 180)] private float lowerLookLimit = 80.0f;

	[Header("Jumping Parameters")]
	[SerializeField] private float jumpForce = 8.0f;
	[SerializeField] private float gravity = 30.0f;

	[Header("Crouch Parameters")]
	[SerializeField] private float crouchHeight = 0.5f;
	[SerializeField] private float standingHeight = 2f;
	[SerializeField] private float timeToCrouch = 0.25f;
	[SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
	[SerializeField] private Vector3 standingCenter = new Vector3(0, 0f, 0);
	private bool isCrouching;
	private bool duringCrounchAnimation;

	[Header("Headbob Parameters")]
	[SerializeField] private float walkBobSpeed = 14f;
	[SerializeField] private float walkBobAmount = 0.05f;
	[SerializeField] private float sprintBobSpeed = 18f;
	[SerializeField] private float sprintBobAmount = 0.11f;
	[SerializeField] private float crouchBobSpeed = 8f;
	[SerializeField] private float crouchBobAmount = 0.025f;
	private float defaultYPos = 0;
	private float timer;

	[Header("Footstep Parameters")]
	[SerializeField] private float baseStepSpeed = 0.45f;
	[SerializeField] private float crouchStepMultiplier = 0.9f;
	[SerializeField] private float sprintStepMultiplier = 0.7f;
	//[SerializeField] private AudioSource audioSource = default;
	[SerializeField] private AudioClip[] woodClips = default;
	[SerializeField] private AudioClip[] dirtClips = default;
	[SerializeField] private AudioClip[] concreteClips = default;
	[SerializeField] private AudioClip[] metalClips = default;
	[SerializeField] private AudioClip[] grassClips = default;
	[SerializeField] private AudioClip[] glassClips = default;
	private float footstepTimer = 0f;
	private float GetCurrentOffset => isCrouching ? baseStepSpeed * crouchStepMultiplier : IsSprinting ? baseStepSpeed * sprintStepMultiplier : baseStepSpeed;


	private Vector3 hitPointNormal;

	private bool isSliding
	{
		get
		{
			//Debug.DrawRay(transform.position, Vector3.down, Color.red);
			if(characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2f))
			{
				hitPointNormal = slopeHit.normal;
				return Vector3.Angle(hitPointNormal, Vector3.up) > characterController.slopeLimit;
			}
			else
			{
				return false;
			}
		}
	}

	private Camera playerCamera;
	private CharacterController characterController;

	public Vector3 moveDirection;
	private Vector2 currentInput;

	private float rotationX = 0;

	private void Update()
	{
		if (CanMove)
		{
			HandleMovementInput();
			HandleMouseLook();

			if(canJump)
				HandleJump();

			if (canCrouch)
				HandleCrouch();

			if (canFootsteps)
				HandleFootsteps();

			if (canUseHeadbob)
				HandleHeadbob();

			ApplyFinalMovement();
		}
	}

	private void HandleMovementInput()
	{
		currentInput = new Vector2((isCrouching ? crouchSpeed : IsSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Vertical"), (isCrouching ? crouchSpeed : IsSprinting ? sprintSpeed : walkSpeed) * Input.GetAxis("Horizontal"));

		float moveDirectionY = moveDirection.y;
		moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
		moveDirection.y = moveDirectionY;
	}

	private void HandleMouseLook()
	{
		rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
		rotationX = Mathf.Clamp(rotationX, -upperLookLimit, lowerLookLimit);
		playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
		transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
	}

	private void HandleJump()
	{
		if (ShouldJump)
			moveDirection.y = jumpForce;
	}

	private void HandleCrouch()
	{
		if (Shouldcrouch)
			StartCoroutine(CrouchStand());
	}

	private void HandleHeadbob()
	{
		if (!characterController.isGrounded) return;

		if(Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
		{
			timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : IsSprinting ? sprintBobSpeed : walkBobSpeed);
			playerCamera.transform.localPosition = new Vector3(
				playerCamera.transform.localPosition.x,
				defaultYPos + Mathf.Sin(timer) * (isCrouching ? crouchBobAmount : IsSprinting ? sprintBobAmount : walkBobAmount),
				playerCamera.transform.localPosition.z);
		}
	}

	private void ApplyFinalMovement()
	{
		if (!characterController.isGrounded)
			moveDirection.y -= gravity * Time.deltaTime;

		if (willSlideOnSlopes && isSliding)
			moveDirection += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * slopeSpeed;

		characterController.Move(moveDirection * Time.deltaTime);
	}

	private IEnumerator CrouchStand()
	{
		if (isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1f))
			yield break;

		duringCrounchAnimation = true;

		float timeElapsed = 0;
		float targetHeight = isCrouching ? standingHeight : crouchHeight;
		float currentHeight = characterController.height;
		Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
		Vector3 currentCenter = characterController.center;

		while(timeElapsed < timeToCrouch)
		{
			characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
			characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
			timeElapsed += Time.deltaTime;
			yield return null;
		}

		characterController.height = targetHeight;
		characterController.center = targetCenter;

		isCrouching = !isCrouching;

		duringCrounchAnimation = false;
	}

	public void LockCursor()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	public void UnlockCursor()
	{
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	private void HandleFootsteps()
	{
		if(!characterController.isGrounded) return;
		if(currentInput == Vector2.zero) return;

		footstepTimer -= Time.deltaTime;
		if(footstepTimer <= 0)
		{
			if(Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 3))
			{
				switch (hit.collider.tag)
				{
					case "Wood":
						audioSource.PlayOneShot(woodClips[Random.Range(0, woodClips.Length - 1)]);
						break;
					case "Concrete":
						audioSource.PlayOneShot(concreteClips[Random.Range(0, concreteClips.Length - 1)]);
						break;
					case "Metal":
						audioSource.PlayOneShot(metalClips[Random.Range(0, metalClips.Length - 1)]);
						break;
					case "Glass":
						audioSource.PlayOneShot(glassClips[Random.Range(0, glassClips.Length - 1)]);
						break;
					case "Grass":
						audioSource.PlayOneShot(grassClips[Random.Range(0, grassClips.Length - 1)]);
						break;
					case "Dirt":
						audioSource.PlayOneShot(dirtClips[Random.Range(0, dirtClips.Length - 1)]);
						break;
					default:
						audioSource.PlayOneShot(dirtClips[Random.Range(0, dirtClips.Length - 1)]);
						break;
				}
			}
			footstepTimer = GetCurrentOffset;
		}
	}
}