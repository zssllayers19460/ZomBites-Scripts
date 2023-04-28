using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anim : MonoBehaviour
{
    private Animator anim;
    private PlayerController controller;

    private void Awake()
    {
        GetSomeReferneces();
    }

    private void Update()
    {
        HandleAnimations();
    }

    private void HandleAnimations()
	{
		if(controller.moveDirection == Vector3.zero)
		{
			anim.SetFloat("Speed", 0f);
		}
		else if(controller.moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
		{
			anim.SetFloat("Speed", 0.5f);
		}
		else if(controller.moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
		{
			anim.SetFloat("Speed", 1f);
		}
	}

    private void GetSomeReferneces()
	{
		anim = GetComponent<Animator>();
        controller = GetComponentInParent<PlayerController>();
	}
}
