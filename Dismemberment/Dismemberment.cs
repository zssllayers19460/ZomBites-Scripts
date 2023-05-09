using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dismemberment : MonoBehaviour
{
    public void GetHit()
    {
        // Set the scale to zero
        transform.localScale = Vector3.zero;
    }
}