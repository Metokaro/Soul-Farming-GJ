using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomExitScript : InteractableObject
{
    RoomGenerator roomGenerator;
    [SerializeField] private TextMeshProUGUI popup;
    Coroutine co;
    public override void Start()
    {
        base.Start();
        roomGenerator = FindObjectOfType<RoomGenerator>();
    }

    public override void OnInteract()
    {
        roomGenerator.OnExitRoom(out int enemiesCount);
        //if(enemiesCount > 0)
        //{
        //    if (co != null) { StopCoroutine(co); }
        //    co = StartCoroutine(ClosePopup());
        //    popup.text = "Clear room first!" + "" + "<br>(Enemies remaining: " + enemiesCount.ToString() + ")" ;
        //}
    }

    IEnumerator ClosePopup()
    {
        popup.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        popup.gameObject.SetActive(false);
    }
}
