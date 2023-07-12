using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class OpenInventory : MonoBehaviour
{
    public static OpenInventory Instance { get; set; }
 
    public GameObject inventoryScreenUI;
    public bool isOpen;

    private PlayerController playerController;
 
    private void Awake()
    {
        GetReferences();
        inventoryScreenUI.SetActive(false);
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
 
    void Start()
    {
        isOpen = false;
    }
 
    void Update()
    {
 
        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {
 
			Debug.Log("I is pressed");
            inventoryScreenUI.SetActive(true);
            isOpen = true;
            playerController.UnlockCursor();
 
        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            isOpen = false;
            playerController.LockCursor();
        }
    }

    private void GetReferences()
    {
        playerController = GetComponentInParent<PlayerController>();
    }
}