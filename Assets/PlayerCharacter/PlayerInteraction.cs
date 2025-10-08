using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    HashSet<InteractableObject> nearbyInteractableObjects = new();
    InteractableObject selectedObject;
    public LayerMask interactableObjLayerMask;
    public GameObject interactionDisplayUI;

    public InteractableObject SelectInteractableObject(Vector3 mousePos)
    {
        InteractableObject interactableObject = null;
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 1, interactableObjLayerMask);
        if(hit && hit.collider.transform.parent.GetComponent<InteractableObject>() != null)
        {
           
            if (nearbyInteractableObjects.Contains(hit.collider.transform.parent.GetComponent<InteractableObject>()))
            {
                interactableObject = hit.collider.transform.parent.GetComponent<InteractableObject>();
                interactableObject.OnSelect();
                interactionDisplayUI.SetActive(true);
                interactionDisplayUI.GetComponent<InteractPanelDisplay>().nameDisplay.text = interactableObject.objectName;
                interactionDisplayUI.GetComponent<InteractPanelDisplay>().tooltipDisplay.text = interactableObject.interactTooltip;

            }

        }
        else if(selectedObject != null)
        {
            selectedObject.OnDeselect();
            interactionDisplayUI.SetActive(false);
            selectedObject = null;
        }
        return interactableObject;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.CompareTag("InteractableObject"))
        {
            collision.transform.parent.GetComponent<InteractableObject>().OnHoverEnter();
            nearbyInteractableObjects.Add(collision.transform.parent.GetComponent<InteractableObject>());
        }
        
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("InteractableObject"))
        {
            collision.transform.parent.GetComponent<InteractableObject>().OnHoverExit();
            interactionDisplayUI.SetActive(false);
            nearbyInteractableObjects.Remove(collision.transform.parent.GetComponent<InteractableObject>());
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        if(SelectInteractableObject(mousePos) != null)
        {
            selectedObject = SelectInteractableObject(mousePos);
              
        }
        
        if (selectedObject != null && Input.GetKeyDown(KeyCode.F))
        {
            selectedObject.OnInteract();
        }
    }
}
