using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyExpImage : MonoBehaviour
{
    public float delay = 8f;
    
    private void Start()
    {
        Destroy(gameObject, delay);
    }
}
