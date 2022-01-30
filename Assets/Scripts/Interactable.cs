using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OffsetOnHover))]
public class Interactable : MonoBehaviour {
    
    public OffsetOnHover hover;

    public virtual void Start() {
    	hover = GetComponent<OffsetOnHover>();
    }

    public void OnClicked() {
        if(GetComponent<HexSpace>().interactable) {
            Interact();
        }   
    }
    
    public virtual void Interact() {

    }
    
//Mouse hover effects
    public void OnMouseEnter() {
        if(GetComponent<HexSpace>().interactable) {
        	StopCoroutine(hover.Deactivate());
        	StartCoroutine(hover.Activate());
        }
    }
    public void OnMouseExit() {
    	StopCoroutine(hover.Activate());
    	StartCoroutine(hover.Deactivate());
    }
//
}