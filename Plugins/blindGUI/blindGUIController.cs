/// <summary>
/// blind GUI - simple GUI framework without editor scripts. Ideal for fast and simple GUI creation.
/// 
/// Simples way is create gameobject for every script to make strict hierarhy like this.
///
/// Controller/blidGUIController
///   - BackgroundLayer / blindGUILayer
///       - BackgroundImage / blindGUIContainer
///   - WelcomeLayer / blindGUILayer
///       - WelcomeContainer / blindGUIContainer
///           - OkButton / blindGUIClickButton
///           - WelcomeLabel / blindGUITextLabel
///           - GreetingsLabel / blindGUITextLabel
///
/// </summary>

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public enum ScaleType {
	NoScale = 0,
	FitAll = 1,
	FitWidth = 2,
	FitHeight = 3,
	FillAll = 4
}

public enum ScaleDirection {
	Both = 0,
	Shrink = 1,
	Grow = 2
}

/// <summary>
/// blind GUI controller class. Controls layering, drawing and arangement of GUI controls.
/// </summary>
public class blindGUIController : MonoBehaviour {
	
	/// <summary>
	/// This flag can be used to scale controls if screen resolution differs from target. 
	/// Usefull for debug game runs inside editor.
	/// </summary>
	public ScaleType m_scaleType;
	
	/// <summary>
	/// Using this flag you can choose direction of scaling
	/// </summary>
	public ScaleDirection m_scaleDirection;
	
	/// <summary>
	/// Target resulution of screen.
	/// </summary>
	public Vector2 m_targetScreenSize;
	
	/// <summary>
	/// Array of GUI objects
	/// </summary>
	private List<blindGUILayer> m_layers = new List<blindGUILayer>();
	
	/// <summary>
	/// Initialization
	/// </summary>
	void Start () {
		UpdateGUIInformation();
	}
	
	/// <summary>
	/// Draws GUI layers from list.
	/// Each layer is screen sized.
	/// </summary>
	void OnGUI() {
		
		if (((m_targetScreenSize.x <=0) && (m_scaleType != ScaleType.FitHeight)) || 
		    ((m_targetScreenSize.y <= 0) && (m_scaleType != ScaleType.FitWidth))) {
			m_scaleType = ScaleType.NoScale;	
		}
		foreach ( blindGUILayer layer in m_layers ) {
			
			switch (m_scaleType) {
				
			case ScaleType.NoScale: {
					
				layer.Draw( new blindGUILayout(new Vector2(Screen.width/2,Screen.height/2), new Vector2(0,0), new Vector2(1,1), new Vector2(Screen.width, Screen.height), 0) , true );	
					
				} break;
			
			case ScaleType.FillAll: {
			
				float scale = 1;
				scale = Mathf.Max( (float)Screen.width/ (float)m_targetScreenSize.x, (float)Screen.height/ (float)m_targetScreenSize.y );
				if (((m_scaleDirection == ScaleDirection.Shrink) && (scale > 1)) || 
					((m_scaleDirection == ScaleDirection.Grow) && (scale < 1))) {
					scale = 1;
				}
				layer.Draw( new blindGUILayout(new Vector2(Screen.width/2,Screen.height/2), new Vector2(0,0), new Vector2(scale,scale), new Vector2(Screen.width, Screen.height)/scale, 0) , true );	
			
				} break;
				
			case ScaleType.FitAll: {
				
				float scale = 1;
				scale = Mathf.Min( (float)Screen.width/ (float)m_targetScreenSize.x, (float)Screen.height/ (float)m_targetScreenSize.y );
				if (((m_scaleDirection == ScaleDirection.Shrink) && (scale > 1)) || 
					((m_scaleDirection == ScaleDirection.Grow) && (scale < 1))) {
					scale = 1;
				}
				layer.Draw( new blindGUILayout(new Vector2(Screen.width/2,Screen.height/2), new Vector2(0,0), new Vector2(scale,scale), new Vector2(Screen.width, Screen.height)/scale, 0) , true );	
				
				} break;
	
				
			case ScaleType.FitHeight: {
				
				float scale = 1;
				scale = (float)Screen.height/ (float)m_targetScreenSize.y;
				if (((m_scaleDirection == ScaleDirection.Shrink) && (scale > 1)) || 
					((m_scaleDirection == ScaleDirection.Grow) && (scale < 1))) {
					scale = 1;
				}	
				layer.Draw( new blindGUILayout(new Vector2(Screen.width/2,Screen.height/2), new Vector2(0,0), new Vector2(scale,scale), new Vector2(Screen.width, Screen.height)/scale, 0) , true );	
				
				} break;
				
			case ScaleType.FitWidth: {
					
				float scale = 1;
				scale = (float)Screen.width/ (float)m_targetScreenSize.x;
				if (((m_scaleDirection == ScaleDirection.Shrink) && (scale > 1)) || 
					((m_scaleDirection == ScaleDirection.Grow) && (scale < 1))) {
					scale = 1;
				}					
				layer.Draw( new blindGUILayout(new Vector2(Screen.width/2,Screen.height/2), new Vector2(0,0), new Vector2(scale,scale), new Vector2(Screen.width, Screen.height)/scale, 0) , true );	
					
					
				} break;
			
			}
		}
	}
	
	/// <summary>
	/// Searches for all included blindGUILayer objects to store them in list.
	/// Also used to update list of layers. This method must be called if you create GUI programmatically.
	/// </summary>
	void UpdateGUIInformation() {
		
		m_layers.Clear();
		
		foreach( blindGUILayer layer in this.gameObject.GetComponentsInChildren<blindGUILayer>() ) {
			if (layer.gameObject != null && layer.gameObject.active) {
				m_layers.Add(layer);
			}
		}
		// Sort layers by Z
		m_layers.Sort((a,b) => {return a.z.CompareTo(b.z);} );
		
		foreach (blindGUILayer layer in m_layers) {
			layer.id = m_layers.IndexOf(layer);	
		}		
	}
}