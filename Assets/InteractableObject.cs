using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableObject : MonoBehaviour
{
    public Material outlineMaterial; 
    public Color selectedOutlineColor;
    public string objectName;
    public string interactTooltip;
    Material defaultMaterial; 
    Color defaultOutlineColor;
  [HideInInspector] public SpriteRenderer spriteRenderer;
    public abstract void OnInteract();

    public virtual void OnHoverEnter()
    {
        defaultMaterial = spriteRenderer.material;
       spriteRenderer.material = outlineMaterial;
        
    }

    public virtual void OnHoverExit()
    {
        spriteRenderer.material = defaultMaterial;
    }

    public virtual void OnSelect()
    {
        defaultOutlineColor = spriteRenderer.material.GetColor("_OutlineColor");
        spriteRenderer.material.SetColor("_OutlineColor", Color.yellow);
    }

    public virtual void OnDeselect()
    {
        spriteRenderer.material.SetColor("_OutlineColor", Color.white);
    }
}
