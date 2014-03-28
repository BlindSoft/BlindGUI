// TODO: Add Header
using UnityEngine;
using System.Collections;
using System;


/// <summary>
/// Layer is a screen-size container which can be shown or hidden with all elements on it.
/// Every layer has it's own layer.
/// </summary>
public class blindGUILayer : blindGUIParentElement {
	
	/// <summary>
	/// Identificator of layer. This is assigned by blindGUIController
	/// </summary>
	public int id;
	
	/// <summary>
	/// Frame of layer
	/// </summary>
	protected blindGUILayout m_screenLayout;
	
	/// <summary>
	/// Initialization
	/// </summary>
	public override void Start () {
		// buildGUIParentElement
		m_horizontalAlign = blindGUIParentElement.HALIGN.stretch;
		m_verticalAlign = blindGUIParentElement.VALIGN.stretch;
		base.Start();
	}
	
	/// <summary>
	/// Creates window for layer
	/// </summary>
	/// <param name="drawRect">
	/// Parent region with <see cref="Rect"/> type. Already scaled.
	/// </param>
	/// <param name="parentScale">
	/// Scale of parent element. Controller send here main scale
	/// </param>
	/// <param name="active">
	/// Flag for element if it is active of <see cref="System.Boolean"/> type.
	/// </param>
	public override void Draw( blindGUILayout parentLayout, bool active ) {
		
		if (m_alpha <= 0.25f) return;
		
		// Get frame of layer
		m_screenLayout = parentLayout;
		
		m_anchorPoint = new Vector2(0,0);
		m_offset = new Vector2(0,0);
		m_size = parentLayout.size;
						
		// Store gui color
		Color guiColor = GUI.color;		
		// Set alpha
		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, m_alpha);
		
		base.Draw( m_screenLayout , (m_alpha >= 0.75f) );
			
		// Restore gui color
		GUI.color = guiColor;
	}
}