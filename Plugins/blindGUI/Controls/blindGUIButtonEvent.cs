using UnityEngine;
using System.Collections;
using System;

public abstract class blindGUIButtonEvent : MonoBehaviour {

	public virtual void OnButtonClick( blindGUIButton sender ) {

	}
	
	public virtual void OnButtonStateChange( blindGUIButton sender, bool newState ) {
		
	}
}