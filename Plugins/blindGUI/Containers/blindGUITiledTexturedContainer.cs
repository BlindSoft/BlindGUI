// TODO: Add Header
using UnityEngine;
using System.Collections;


/// <summary>
/// This class used as container for objects
/// </summary>
public class blindGUITiledTexturedContainer : blindGUITexturedContainer {
	
	/// <summary>
	/// Size of tiled texture
	/// </summary>
	protected float m_tiledTextureWidth = 0;
	protected float m_tiledTextureHeight = 0;
	
	/// <summary>
	/// Tiled texture
	/// </summary>
	protected Texture2D m_tiledTexture;
	
	/// <summary>
	/// Original texture
	/// </summary>
	protected Texture2D m_originalBackgroundTexture;
	
	/// <summary>
	/// Initialization
	/// </summary>
	public override void Start () {
		
		base.Start();
		ReassignTexture();
	}
	
	/// <summary>
	/// Call this method immediatly after m_backgroundTexture was changed at runtime
	/// </summary>
	public void ReassignTexture() {
		m_originalBackgroundTexture = m_backgroundTexture as Texture2D;	
		UpdateTexture();
	}
	
	/// <summary>
	/// Updated tiled texture information
	/// </summary>
	protected void UpdateTexture() {
		
		if ((!m_originalBackgroundTexture) || (m_originalBackgroundTexture.GetType() != typeof(Texture2D))) return;
		
		int n = Mathf.CeilToInt(m_tiledTextureWidth / m_originalBackgroundTexture.width);
		int m = Mathf.CeilToInt(m_tiledTextureHeight / m_originalBackgroundTexture.height);

		m_tiledTexture = new Texture2D(n*m_originalBackgroundTexture.width,m*m_originalBackgroundTexture.height);
		for (int i=0; i<n;i++) {
			for (int j=0; j<m;j++) {
				
				m_tiledTexture.SetPixels(i*m_originalBackgroundTexture.width,j*m_originalBackgroundTexture.height,m_originalBackgroundTexture.width, m_originalBackgroundTexture.height,
		                         ((Texture2D)m_originalBackgroundTexture).GetPixels());
		        
			}
		}
		m_tiledTexture.Apply();
		m_backgroundTexture = m_tiledTexture;
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
		
		m_textureScaleMode = ScaleMode.ScaleAndCrop;
		
		Rect containerFrame = GetFrame(parentLayout);
		
		if ((containerFrame.width != 0) && (containerFrame.height != 0) && (m_backgroundTexture != null)) {
		
			if ((containerFrame.width != m_tiledTextureWidth) || 
			    (containerFrame.height != m_tiledTextureHeight)) {
				
				m_tiledTextureWidth = containerFrame.width;
				m_tiledTextureHeight = containerFrame.height;
				
				UpdateTexture();			
			}
		}
		
		base.Draw(parentLayout, active);
	}
}