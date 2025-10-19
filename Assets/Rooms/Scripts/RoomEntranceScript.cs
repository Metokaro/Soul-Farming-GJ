using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEntranceScript : InteractableObject
{
    public RoomGenerator roomGenerator;
    public void Awake()
    {
        roomGenerator = FindObjectOfType<RoomGenerator>();
    }
    public override void OnInteract()
    {
        roomGenerator.OnEnterRoom();
    }
}
