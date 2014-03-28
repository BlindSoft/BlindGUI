// TODO: Add Header
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


/// <summary>
/// Base class for buttons
/// </summary>
public class blindGUIButton : blindGUIParentElement {
	
	
	/// <summary>
	/// This delegate is called when button is clicked
	/// </summary>
	public delegate void ButtonClickDelegate( blindGUIButton sender );
	/// <summary>
	/// This delefate is called when button state changed
	/// </summary>
	public delegate void ButtonStateChanged( blindGUIButton sender, bool newState );
	
	/// <summary>
	/// Buttons type enumeration
	/// </summary>
	public enum BUTTON_TYPE {
		ClickButton,
		ToggleButton,
		RepeatButton,
		RadioButton
	}
	
	/// <summary>
	/// Type of button
	/// </summary>
	public BUTTON_TYPE m_buttonType;
	
	/// <summary>
	///  Buttons states enumeration
	/// </summary>
	public enum BUTTON_STATE {
		Idle,
		Pressed,
		Disabled
	}
	
	/// <summary>
	/// Button is in push state
	/// </summary>
	public bool m_pushed = false;
	
	/// <summary>
	/// Previous state of toggle button
	/// </summary>
	protected bool m_prevPushState;
	
	/// <summary>
	/// Public button state
	/// </summary>
	protected BUTTON_STATE m_buttonState;
	
	
	/// <summary>
	/// Image on button when it is active
	/// </summary>
	public Texture2D m_idleImage;
	/// <summary>
	/// Image on button when it is pressed
	/// </summary>
	public Texture2D m_pressImage;
	/// <summary>
	/// Image on button when mouse hovers it
	/// </summary>
	public Texture2D m_hoverImage;
	/// <summary>
	/// Image on button when it is disabled
	/// </summary>
	public Texture2D m_disabledImage;
	
	/// <summary>
	/// Automatically adjust size of button if true
	/// </summary>
	public bool m_autoSizeToTexture = true;
	
	/// <summary>
	/// Buttons grounp tag for toggle mode to make radio buttons
	/// </summary>
	public string m_groupTag;
	
	/// <summary>
	/// List of radiobuttons in this group
	/// </summary>
	protected List<blindGUIButton> m_radiobuttonsInGroup = new List<blindGUIButton>();
	
	/// <summary>
	/// Button click event
	/// </summary>
	public ButtonClickDelegate m_buttonClickDelegate;
	
	/// <summary>
	/// Button state changed event
	/// </summary>
	public ButtonStateChanged m_buttonStateChangedDelegate;
	
	public override void Draw( blindGUILayout parentLayout, bool active ) {
		
		// If autosize flag is true, resize button based on idle texture
		if (m_autoSizeToTexture) {
			if (m_idleImage != null) {
				m_size.x = m_idleImage.width;
				m_size.y = m_idleImage.height;
			}
		}
		
		SetButtonGraphics( active & m_enabled );
		Rect buttonFrame = GetFrame(parentLayout);
		
		if (m_enabled && active) {
			// Toggle button
			if (m_buttonType == blindGUIButton.BUTTON_TYPE.ToggleButton) {
				m_pushed = GUI.Toggle(buttonFrame, m_pushed, "",m_style);
				
				if (m_pushed) {				
					m_buttonState = blindGUIButton.BUTTON_STATE.Pressed;	
				} else {
					m_buttonState = blindGUIButton.BUTTON_STATE.Idle;	
				}
				
				if ((m_prevPushState != m_pushed) && (m_buttonStateChangedDelegate != null)) {
					m_buttonStateChangedDelegate(this, m_pushed);
				}
				
				m_prevPushState = m_pushed;
			// Radio Button
			} else if (m_buttonType == blindGUIButton.BUTTON_TYPE.RadioButton) {
				
				bool m_stateBeforePush = m_pushed;
				m_pushed = GUI.Toggle(buttonFrame, m_pushed, "",m_style);
				
				if (m_stateBeforePush && !m_pushed) {
					m_pushed = true;	
				}
				
				if (!m_stateBeforePush && m_pushed) {
					m_buttonState = blindGUIButton.BUTTON_STATE.Pressed;
					foreach (blindGUIButton button in m_radiobuttonsInGroup) {
						button.m_pushed = false;
						if (button.m_buttonStateChangedDelegate != null) {
							button.m_buttonStateChangedDelegate( button, false);
						}
					}
					if (m_buttonStateChangedDelegate != null) {
						m_buttonStateChangedDelegate(this, m_pushed);
					}
				}
				
				if (m_pushed) {				
					m_buttonState = blindGUIButton.BUTTON_STATE.Pressed;	
				} else {
					m_buttonState = blindGUIButton.BUTTON_STATE.Idle;	
				}
				
				//m_prevPushState = m_pushed;
				
			// Repeat Button
			} else if (m_buttonType == blindGUIButton.BUTTON_TYPE.RepeatButton) {
				m_buttonState = blindGUIButton.BUTTON_STATE.Idle;
				bool buttonResult = GUI.RepeatButton(buttonFrame, "", m_style);
				if (buttonResult && (m_buttonClickDelegate != null)) {
					m_buttonClickDelegate(this);	
				}
	
			// Default click button
			} else {
				m_buttonState = blindGUIButton.BUTTON_STATE.Idle;	
				bool buttonResult = GUI.Button(buttonFrame, "", m_style);
				if (buttonResult && (m_buttonClickDelegate != null)) {
					m_buttonClickDelegate(this);	
				}
			}
		} else {
			GUI.Button(buttonFrame, "", m_style);
		}
				
		base.Draw(parentLayout, active & m_enabled);
	}
	
	protected void SetButtonGraphics( bool active ) {
		
		if (!m_pressImage) m_pressImage = m_idleImage;
		if (!m_hoverImage) m_hoverImage = m_idleImage;
		if (!m_disabledImage) m_disabledImage = m_idleImage;
		
		if (active) {
			m_style.active.background = m_pressImage;
			m_style.hover.background = m_hoverImage;
			if (m_buttonState == blindGUIButton.BUTTON_STATE.Pressed) {
				m_style.normal.background = m_pressImage;
			} else {
				m_style.normal.background = m_idleImage;
			}
		} else {
			m_style.active.background = m_disabledImage;
			m_style.hover.background = m_disabledImage;
			m_style.normal.background = m_disabledImage;
		}
	}
	
	override public void UpdateLayout() {
		base.UpdateLayout();
		
		if (m_buttonType == blindGUIButton.BUTTON_TYPE.RadioButton) {
			m_radiobuttonsInGroup.Clear();
			blindGUIButton [] allButtons = GameObject.FindObjectsOfType(typeof(blindGUIButton)) as blindGUIButton[];
			foreach (blindGUIButton button in allButtons) {
				if ((button.m_groupTag == m_groupTag) && (button.m_buttonType == blindGUIButton.BUTTON_TYPE.RadioButton) && (button != this)) {
					m_radiobuttonsInGroup.Add(button);	
				}
			}
		}
	}
	
}