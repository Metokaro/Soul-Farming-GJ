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
    public virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public virtual void OnEnterRange()
    {
        defaultMaterial = spriteRenderer.material;
        spriteRenderer.material = outlineMaterial;
        
    }

    public virtual void OnExitRange()
    {
        spriteRenderer.material = defaultMaterial;
    }

    public virtual void OnMouseHover()
    {
        defaultOutlineColor = spriteRenderer.material.GetColor("_OutlineColor");
        spriteRenderer.material.SetColor("_OutlineColor", Color.yellow);
    }

    public virtual void OnMouseLeave()
    {
        spriteRenderer.material.SetColor("_OutlineColor", Color.white);
    }
}

public interface IMachine
{

}
