// TODO: Add Header
using UnityEngine;
using System.Collections;

/// <summary>
/// This class used as container for objects
/// </summary>
public class blindGUIColorTexturedContainer : blindGUITiledTexturedContainer {
	
	public Color m_textureColor;
	protected Color m_currentTextureColor;
	protected Texture2D onePixelTexture;
	
	override public void Start() {
		onePixelTexture = new Texture2D(1,1);
		CreateTiledColorTexture();
		m_textureScaleMode = ScaleMode.StretchToFill;
		base.Start();
	}
	
	/// <summary>
	/// Draws current element and all children.
	/// </summary>
	/// <param name="drawRect">
	/// Parent region with <see cref="Rect"/> type. Already scaled.
	/// </param>
	/// <param name="active">
	/// Flag for element if it is active of <see cref="System.Boolean"/> type.
	/// </param>
	public override void Draw( blindGUILayout parentLayout, bool active ) {
		CreateTiledColorTexture();
		m_backgroundTexture = onePixelTexture;
		m_textureScaleMode = ScaleMode.StretchToFill;
		base.Draw( parentLayout, active );
	}
	// Updates color of texture
	protected void CreateTiledColorTexture() {
		
		if (m_currentTextureColor != m_textureColor) {
			onePixelTexture.SetPixel(0,0,m_textureColor);
			onePixelTexture.Apply();
			m_currentTextureColor = m_textureColor;
		}
		
				
	}
}
