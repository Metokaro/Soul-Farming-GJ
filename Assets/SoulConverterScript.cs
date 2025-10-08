using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulConverterScript : InteractableObject
{
    public override void OnInteract()
    {
        Debug.Log("Convert soul");
    }

    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
