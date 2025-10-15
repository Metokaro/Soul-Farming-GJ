using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomExitScript : InteractableObject
{
    RoomGenerator roomGenerator;
    public override void Start()
    {
        base.Start();
        roomGenerator = FindObjectOfType<RoomGenerator>();
    }
    public override void OnInteract()
    {
        roomGenerator.OnExitRoom();
    }
}
