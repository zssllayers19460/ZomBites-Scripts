using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootItemScriptableObject : ScriptableObject
{
    public new string lootName;
    public string description;
    public Sprite lootIcon;

    public virtual void Use()
    {
        //Debug.Log(name + " was used");
    }
}