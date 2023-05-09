using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave 
{
    public string name;
    public int enemiesAmmount;
    public float delay = 0.7f;
    public List<GameObject> enemy;    
}
