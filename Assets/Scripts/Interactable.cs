using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class for all interactable gridbased elements to derrive from
//Doesn't serve much purpose in this execution, but will be helpful later on...
[RequireComponent(typeof(OffsetOnHover))]
public class Interactable : MonoBehaviour {
    
//Hover effect definitions
    public OffsetOnHover hover;
    public virtual void Start() {
    	hover = GetComponent<OffsetOnHover>();
    }
//
//Function executed through CameraController class, checks if interactable is unlocked and interacts
    public void OnClicked() {
        if(GetComponent<HexSpace>().interactable) { //sketchy ref. makes base class hex specific 
            Interact();
        }   
    }
//...ghost town... maybe this will be useful later...
    public virtual void Interact() {

    }
//   
//Mouse hover effects using OffsetOnHover class
//These might be better suited for the TileFlip class... but here they are
    public void OnMouseEnter() {
        if(GetComponent<HexSpace>().interactable) {
        	StopCoroutine(hover.Deactivate());
        	StartCoroutine(hover.Activate());
        }
    }
    public void OnMouseExit() { //Getting some edge cases where colliders aren't registering the mouse leaving? Or is it not stopping the coroutine how I think it is?
    	StopCoroutine(hover.Activate());
    	StartCoroutine(hover.Deactivate());
    }
//
}