using UnityEngine;
using System.Collections;

public class blindGUITexturedContainer : blindGUIParentElement {
	
	/// <summary>
	/// Texture for background of component
	/// </summary>
	public Texture m_backgroundTexture;
	
	/// <summary>
	/// Set size of control automatically
	/// </summary>
	public bool m_autoSizeToTexture;
	
	/// <summary>
	/// Texture Scale Mode
	/// </summary>
	public ScaleMode m_textureScaleMode = ScaleMode.ScaleToFit;

	
	public override void Draw( blindGUILayout parentLayout, bool active ) {
	
		// If autosize flag is true, resize button based on idle texture
		if (m_autoSizeToTexture) {
			if (m_backgroundTexture != null) {
				m_size.x = m_backgroundTexture.width;
				m_size.y = m_backgroundTexture.height;
			}
		}
		Rect containerFrame = GetFrame(parentLayout);
		
		if (m_backgroundTexture) {
			GUI.DrawTexture(containerFrame,m_backgroundTexture,m_textureScaleMode);
		}
		base.Draw(parentLayout, active);
	}
	
	public override void UpdateLayout ()
	{
		if (m_autoSizeToTexture) {
			if (m_backgroundTexture != null) {
				m_size.x = m_backgroundTexture.width;
				m_size.y = m_backgroundTexture.height;
			}
		}
		base.UpdateLayout ();
	}

}