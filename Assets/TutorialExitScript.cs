using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialExitScript : InteractableObject
{
    public RoomPicker roomPicker;
    public override void OnInteract()
    {
        FinishedTutorial();
    }

    public void FinishedTutorial()
    {
        roomPicker.FinishTutoial();
        Destroy(transform.parent.gameObject);
    }
}
