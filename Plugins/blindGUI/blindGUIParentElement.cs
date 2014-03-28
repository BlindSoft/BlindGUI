// TODO: Add Header
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This is base class for all GUI elements. You can inherit it to create own GUI elements
/// </summary>
public class blindGUIParentElement : MonoBehaviour {

	/// <summary>
	/// Delegate to be called at the end of animation
	/// </summary>
	public delegate void AnimationCompleteDelegate( blindGUIParentElement sender );
	
	/// <summary>
	/// Element layout configuration
	/// </summary>
	protected class _blindGUIElementConfig {
		public readonly Vector2 scale;
		public readonly Vector2 anchorPoint;
		public readonly Vector2 offset;
		public readonly Vector2 size;
		public readonly float angle;
		
		public _blindGUIElementConfig( Vector2 _scale, Vector2 _anchorPoint, Vector2 _offset, Vector2 _size, float _angle ) {
			scale = _scale;
			anchorPoint = _anchorPoint;
			offset = _offset;
			size = _size;
			angle = _angle;
		}
		
	}
	
	/// <summary>
	/// Horizontal alignment of element;
	/// </summary>
	public enum HALIGN { left, center, right, stretch, free };
	public HALIGN m_horizontalAlign = HALIGN.left;
	
	/// <summary>
	/// Vertical alignment of element.
	/// </summary>
	public enum VALIGN { top, center, bottom, stretch, free };
	public VALIGN m_verticalAlign = VALIGN.top;
	
	
	/// <summary>
	/// Position of elements anchor point in parent's scale
	/// 
	/// For "top" alignment m_offset.y sets anchor point position relative to top of container
	/// For "bottom" alignment m_offset.y sets anchor point position relative to bottom of container
	/// 
	/// The same effect for left, right alignment and m_offset.x
	/// 
	/// For "center" alignment m_offset.x and m_offset.y sets anchor point position relative to center of container.
	/// 
	/// </summary>
	public Vector2 m_offset = new Vector2(0,0);
	
	/// <summary>
	/// Size of control
	/// </summary>
	public Vector2 m_size = new Vector2(100,100);
	
	/// <summary>
	/// Z-order of control in container(not global). 
	/// </summary>
	public int z = 0;
	
	/// <summary>
	/// Scale of current element
	/// </summary>
	public float m_scale = 1;
	
	/// <summary>
	/// Angle of rotation
	/// </summary>
	public float m_angle;
	
	/// <summary>
	/// Anchor point for rotation
	/// </summary>
	public Vector2 m_anchorPoint;
	
	/// <summary>
	/// Alpha of current element
	/// </summary>
	public float m_alpha = 1.0f;
	
	/// <summary>
	/// Animation speed
	/// </summary>
	public float m_animationTime = 1.0f;
	
	/// <summary>
	/// If this element is active
	/// </summary>
	public bool m_enabled = true;
	
	/// <summary>
	/// Sorted list of 
	/// </summary>
	private List<blindGUIParentElement> m_elements = new List<blindGUIParentElement>();
	
	/// <summary>
	/// Style of element
	/// </summary>
	protected GUIStyle m_style = new GUIStyle();
	
	/// <summary>
	/// Set to true on transform
	/// </summary>
	protected bool m_transformed;
	
	/// <summary>
	/// Original GUI Matrix
	/// </summary>
	protected Matrix4x4 m_guiMatrix;
		
	protected Vector2 m_CSShift;
	
	/// <summary>
	/// Container clipping on/off
	/// </summary>
	public bool m_clipping  = false;
	
	/// <summary>
	/// Sets to strue if clip assigned
	/// </summary>
	private bool m_clipAssigned = false;

	/// <summary>
	/// Original GUI color
	/// </summary>
	private Color m_originalGUIColor;

	/// <summary>
	/// Flag show if color was changed
	/// </summary>
	private bool m_colorChanged;

	/// <summary>
	/// State at start of animation
	/// </summary>
	private blindGUIAnimationState m_startAnimationState;
	/// <summary>
	/// State at end of animation
	/// </summary>
	private blindGUIAnimationState m_finishAnimationState;
	
	/// <summary>
	/// This function will be called at the end of animation
	/// </summary>
	private AnimationCompleteDelegate m_animationCompleteDelegate;
	
	private Vector2 acnhorPointOnScreen = new Vector2(0,0);
	
	/// <summary>
	/// Initialization
	/// </summary>
	public virtual void Start () {
		
		UpdateLayout();	
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
	public virtual void Draw( blindGUILayout parentLayout, bool active) {
		//Debug.Log(this.name+" parent layout: "+parentLayout.ToString());
		blindGUILayout currentLayout;
		Rect containerRectangle = SetupLayout( parentLayout, out currentLayout );
		
		if (this.GetType() == typeof(blindGUIParentElement)) {
			GUI.Box(containerRectangle,this.name);
		}
		
		foreach( blindGUIParentElement element in m_elements) {
				element.Draw(currentLayout, m_enabled & active);	
		}
		
		if (m_clipAssigned) {
			GUI.EndGroup();
			m_clipAssigned = false;
		}
	
		if (m_colorChanged) {
			GUI.color = m_originalGUIColor;
			m_colorChanged = false;
		}
	}
	/// <summary>
	/// Returns frame for current element
	/// </summary>
	/// <param name="parentLayout">
	/// Parent layout of <see cref="blindGUILayout"/> type.
	/// </param>
	/// <returns>
	/// Current elements's frame. <see cref="Rect"/> type;
	/// </returns>
	protected Rect GetFrame( blindGUILayout parentLayout ) {
		blindGUILayout temp;
		return SetupLayout( parentLayout, out temp );
	}
	
	/// <summary>
	/// Returns current element configuration
	/// </summary>
	/// <param name="parentLayout">
	/// Layout of parent element of  <see cref="blindGUILayout"/> type
	/// </param>
	/// <returns>
	/// Returns configuration for this element of <see cref="blindGUIElementConfig"/> type.
	/// </returns>
	private blindGUIElementConfig GetElement( blindGUILayout parentLayout ) {
		
		float   _angle = m_angle;
		Vector2 _anchorPoint = m_anchorPoint;
		Vector2 _size = m_size;
		
		Vector2 _offset = m_offset;
		float   _scale = m_scale;
		
		//-------------------------------------------------
		// Fix angle
		// Set parameters according to alignment
		if ((m_horizontalAlign != blindGUIParentElement.HALIGN.free) || (m_verticalAlign != blindGUIParentElement.VALIGN.free)) {
			_angle = 0;
		}
		
		//-------------------------------------------------
		// Fix anchor point
		// Horizontal
			   if (m_horizontalAlign == blindGUIParentElement.HALIGN.left) {
			_anchorPoint.x = 0f;	
		} else if (m_horizontalAlign == blindGUIParentElement.HALIGN.center) {
			_anchorPoint.x = 0;
		} else if (m_horizontalAlign == blindGUIParentElement.HALIGN.stretch) {
			_anchorPoint.x = 0;	
		} else if (m_horizontalAlign == blindGUIParentElement.HALIGN.right) {
			_anchorPoint.x = 0f;	
		}
		// Vertical
			   if (m_verticalAlign == blindGUIParentElement.VALIGN.top) {
			_anchorPoint.y = 0f;	
		} else if (m_verticalAlign == blindGUIParentElement.VALIGN.center) {
			_anchorPoint.y = 0;
		} else if (m_verticalAlign == blindGUIParentElement.VALIGN.stretch) {
			_anchorPoint.y = 0;	
		} else if (m_verticalAlign == blindGUIParentElement.VALIGN.bottom) {
			_anchorPoint.y = 0f;	
		}
				
		
		//-------------------------------------------------
		// Fix size
		// Horizontal
		if (m_horizontalAlign == blindGUIParentElement.HALIGN.stretch) {
			_size.x = parentLayout.size.x / _scale;			
		}
		// Vertical
		if (m_verticalAlign == blindGUIParentElement.VALIGN.stretch) {
			_size.y = parentLayout.size.y / _scale;			
		}
		
		//-------------------------------------------------
		// Fix offset
		// Horizontal
		
		if (m_horizontalAlign == blindGUIParentElement.HALIGN.left) {
			_offset.x += -parentLayout.size.x*(0.5f+parentLayout.anchorPoint.x)+_size.x*0.5f*_scale;
		} else if (m_horizontalAlign == blindGUIParentElement.HALIGN.center) {
			_offset.x += -parentLayout.size.x*(0.5f+parentLayout.anchorPoint.x-0.5f);
		} else if (m_horizontalAlign == blindGUIParentElement.HALIGN.right) {
			_offset.x = -parentLayout.size.x*(0.5f+parentLayout.anchorPoint.x-1.0f) - _size.x*_scale*0.5f - _offset.x;
		} else if (m_horizontalAlign == blindGUIParentElement.HALIGN.stretch) {
			_offset.x = -parentLayout.size.x*(0.5f+parentLayout.anchorPoint.x-0.5f);
		}
		
		// Vertical
		if (m_verticalAlign == blindGUIParentElement.VALIGN.top) {
			_offset.y += -parentLayout.size.y*(0.5f+parentLayout.anchorPoint.y)+_size.y*0.5f*_scale;
		} else if (m_verticalAlign == blindGUIParentElement.VALIGN.center) {
			_offset.y += -parentLayout.size.y*(0.5f+parentLayout.anchorPoint.y-0.5f);
		} else if (m_verticalAlign == blindGUIParentElement.VALIGN.bottom) {
			_offset.y = -parentLayout.size.y*(0.5f+parentLayout.anchorPoint.y-1.0f) - _size.y*_scale*0.5f - _offset.y;	
		} else if (m_verticalAlign == blindGUIParentElement.VALIGN.stretch) {
			_offset.y = -parentLayout.size.y*(0.5f+parentLayout.anchorPoint.y-0.5f);
		}
		
		return new blindGUIElementConfig( new Vector2(_scale,_scale), 
		                                 _anchorPoint, 
		                                 _offset, 
		                                 _size, 
		                                 _angle);
	}
	
	/// <summary>
	/// Sets rotation, scale and arranges current element
	/// </summary>
	/// <param name="parentLayout">
	/// Layout of parent element of  <see cref="blindGUILayout"/> type
	/// </param>
	/// <param name="elementConfig">
	/// Current element configuration of <see cref="blindGUIElementConfig"/> type
	/// </param>
	/// <param name="currentLayout">
	/// Current element layout for hierarhy. <see cref="blindGUILayout"/> type
	/// </param>
	/// <returns>
	/// Rectaingle of current element. <see cref="Rect"/> type.
	/// </returns>
	private Rect SetupLayout( blindGUILayout parentLayout, out blindGUILayout currentLayout ) {
		
		if (m_angle != 0) m_clipping = false;
		if (m_scale == 0) m_scale = 0.0001f;
		
		blindGUIElementConfig elementConfig = GetElement(parentLayout); //new blindGUIElementConfig( new Vector2(m_scale,m_scale), m_rotationAnchorPoint, m_offset, new Vector2( m_size.x, m_size.y), m_angle);
		
		if (!m_clipAssigned) {
			GUI.matrix = Matrix4x4.identity;
		}
		
		Vector2 realAnchorPoint = new Vector2( -elementConfig.size.x*(0.5f+elementConfig.anchorPoint.x), -elementConfig.size.y*(0.5f+elementConfig.anchorPoint.y) );
	
		Vector2 currentAnchorPoint = new Vector2( parentLayout.realAnchorPoint.x + elementConfig.offset.x*parentLayout.scale.x*Mathf.Cos(-parentLayout.angle*Mathf.Deg2Rad) +
		                                         						       elementConfig.offset.y*parentLayout.scale.y*Mathf.Sin(-parentLayout.angle*Mathf.Deg2Rad)
		                                         , parentLayout.realAnchorPoint.y +elementConfig.offset.y*parentLayout.scale.y*Mathf.Cos(-parentLayout.angle*Mathf.Deg2Rad) - 
		                                         						       elementConfig.offset.x*parentLayout.scale.x*Mathf.Sin(-parentLayout.angle*Mathf.Deg2Rad));
		
		acnhorPointOnScreen = currentAnchorPoint+realAnchorPoint;
		// Current element scale
		Vector2 currentScale = new Vector2(parentLayout.scale.x*elementConfig.scale.x, parentLayout.scale.y*elementConfig.scale.y);
		// Current element angle
		float currentAngle = parentLayout.angle+elementConfig.angle;
		
		if (!m_clipAssigned) {
			GUIUtility.ScaleAroundPivot(currentScale, currentAnchorPoint);
			GUIUtility.RotateAroundPivot(currentAngle, currentAnchorPoint);
		}
	
		if (m_clipping) {
			currentLayout = new blindGUILayout( -realAnchorPoint, m_anchorPoint, currentScale, elementConfig.size, currentAngle );
		} else {
			currentLayout = new blindGUILayout( currentAnchorPoint, m_anchorPoint, currentScale, elementConfig.size, currentAngle );	
		}
		Rect resultRect = new Rect( currentAnchorPoint.x+realAnchorPoint.x, currentAnchorPoint.y +realAnchorPoint.y, elementConfig.size.x, elementConfig.size.y);
		
		if (m_clipping) {
			
			Rect clipRect = new Rect(currentAnchorPoint.x+realAnchorPoint.x,currentAnchorPoint.y+realAnchorPoint.y, elementConfig.size.x+20, elementConfig.size.y+20);
			
			if (!m_clipAssigned) {
				GUI.BeginGroup( clipRect , "", new GUIStyle() );
				m_clipAssigned = true;
			}			
			
			resultRect = new Rect(0,0, elementConfig.size.x, elementConfig.size.y);
		}
		
		if (!m_colorChanged) {
			m_originalGUIColor = GUI.color;
			GUI.color = new Color( m_originalGUIColor.r, m_originalGUIColor.g, m_originalGUIColor.b, m_originalGUIColor.a * m_alpha );
			m_colorChanged = true;
		}
		return resultRect;		
		
	}
	
	
	/// <summary>
	/// Searches all included blindGUILayer objects to store them in list.
	/// Also used to update list of layers. This method must be called if you create GUI programmatically.
	/// </summary>
	virtual public void UpdateLayout() {
		
		m_elements.Clear();
		
		
		/// Look for children of this element and check if them has GUI elements
		foreach(Transform transform in this.transform) {
			
			if (transform.gameObject != null && transform.gameObject.active) {
				blindGUIParentElement[] elements = transform.gameObject.GetComponents<blindGUIParentElement>();
				m_elements.AddRange(elements);
				foreach (blindGUIParentElement element in elements) {
					element.UpdateLayout();	
				}
			}
		}
		
		// Sort elements by Z
		m_elements.Sort((a,b) => {return a.z.CompareTo(b.z);} );	
	}

	/// <summary>
	/// Starts Animation from objects current state to new
	/// </summary>
	/// <param name="targetState">
	/// Target animation state <see cref="blindGUIAnimationState"/>
	/// </param>
	/// <param name="animationTime">
	/// Duration of animation <see cref="System.Single"/>
	public void AnimateTo( blindGUIAnimationState targetState, float animationTime) {
		AnimateTo(targetState, animationTime, null, iTweenInBlindGUI.EaseType.linear, 0.0f);
	}	
	
	/// <summary>
	/// Starts Animation from objects current state to new
	/// </summary>
	/// <param name="targetState">
	/// Target animation state <see cref="blindGUIAnimationState"/>
	/// </param>
	/// <param name="animationTime">
	/// Duration of animation <see cref="System.Single"/>
	/// <param name="easeType">
	/// Animation Ease type <see cref="iTweenInBlindGUI.EaseType"/>
	/// </param>
	public void AnimateTo( blindGUIAnimationState targetState, float animationTime, iTweenInBlindGUI.EaseType easeType ) {
		AnimateTo(targetState, animationTime, null, easeType, 0.0f);
	}	
	
	/// <summary>
	/// Starts Animation from objects current state to new
	/// </summary>
	/// <param name="targetState">
	/// Target animation state <see cref="blindGUIAnimationState"/>
	/// </param>
	/// <param name="animationTime">
	/// Duration of animation <see cref="System.Single"/>
	/// </param>
	/// <param name="animationCompleteDelegate">
	/// This function will be called after animation is finished <see cref="AnimationCompleteDelegate"/>
	/// </param>
	/// <param name="easeType">
	/// Animation Ease type <see cref="iTweenInBlindGUI.EaseType"/>
	/// </param>
	public void AnimateTo( blindGUIAnimationState targetState, float animationTime, AnimationCompleteDelegate animationCompleteDelegate, iTweenInBlindGUI.EaseType easeType ) {
		AnimateTo(targetState, animationTime, animationCompleteDelegate, easeType, 0.0f);
	}

	/// <summary>
	/// Starts Animation from objects current state to new
	/// </summary>
	/// <param name="targetState">
	/// Target animation state <see cref="blindGUIAnimationState"/>
	/// </param>
	/// <param name="animationTime">
	/// Duration of animation <see cref="System.Single"/>
	/// </param>
	/// <param name="animationCompleteDelegate">
	/// This function will be called after animation is finished <see cref="AnimationCompleteDelegate"/>
	/// </param>	
	public void AnimateTo( blindGUIAnimationState targetState, float animationTime, AnimationCompleteDelegate animationCompleteDelegate ) {
		AnimateTo(targetState, animationTime, animationCompleteDelegate, iTweenInBlindGUI.EaseType.linear, 0.0f);	
	}	
	
	/// <summary>
	/// Starts Animation from objects current state to new
	/// </summary>
	/// <param name="targetState">
	/// Target animation state <see cref="blindGUIAnimationState"/>
	/// </param>
	/// <param name="animationTime">
	/// Duration of animation <see cref="System.Single"/>
	/// </param>
	/// <param name="animationCompleteDelegate">
	/// This function will be called after animation is finished <see cref="AnimationCompleteDelegate"/>
	/// </param>
	/// <param name="easeType">
	/// Animation Ease type <see cref="iTweenInBlindGUI.EaseType"/>
	/// </param>
	/// <param name="delay">
	/// Delay of animation <see cref="System.Single"/>
	/// </param>
	public void AnimateTo( blindGUIAnimationState targetState, float animationTime, AnimationCompleteDelegate animationCompleteDelegate, iTweenInBlindGUI.EaseType easeType, float delay ) {
		
		m_animationCompleteDelegate = animationCompleteDelegate;
		m_animationTime = animationTime;
		m_startAnimationState = new blindGUIAnimationState( this );
		m_finishAnimationState = targetState;
	
		//iTweenInBlindGUI.StopByName(this.name+"_animation");
		iTweenInBlindGUI.ValueTo( this.gameObject, iTweenInBlindGUI.Hash(
	                                                             					"name", this.name+"_animation",
	                                                                                "time", m_animationTime,
	                                                                                "delay", delay,
	                                                                                "from", 0,
	                                                                                "to", 1,
		                                                                 			"easetype", easeType,
	                                                                                "onupdate", "AnimationStep",
	                                                                                "oncomplete", "AnimationFinished"));
	                   
	}
	
	
	/// <summary>
	/// New animation Step
	/// </summary>
	/// <param name="t">
	/// A <see cref="System.Single"/>
	/// </param>
	private void AnimationStep( float t ) {
		if (m_startAnimationState.size != m_finishAnimationState.size) {
			this.m_size = m_startAnimationState.size*(1-t)+m_finishAnimationState.size*t;
		}
		if (m_startAnimationState.angle != m_finishAnimationState.angle) {
			this.m_angle = m_startAnimationState.angle*(1-t)+m_finishAnimationState.angle*t;
		}
		if (m_startAnimationState.offset != m_finishAnimationState.offset) {
			this.m_offset = m_startAnimationState.offset*(1-t)+m_finishAnimationState.offset*t;
		}
		if (m_startAnimationState.anchorPoint != m_finishAnimationState.anchorPoint) {
			this.m_anchorPoint = m_startAnimationState.anchorPoint*(1-t)+m_finishAnimationState.anchorPoint*t;
		}
		if (m_startAnimationState.alpha != m_finishAnimationState.alpha) {
			this.m_alpha = m_startAnimationState.alpha*(1-t)+m_finishAnimationState.alpha*t;
		}
		if (m_startAnimationState.scale != m_finishAnimationState.scale) {
			this.m_scale = m_startAnimationState.scale*(1-t)+m_finishAnimationState.scale*t;
		}
	}
	
	/// <summary>
	/// Animation finished. Calling delegate if it is registered
	/// </summary>
	private void AnimationFinished() {
		if (m_animationCompleteDelegate != null) {
			m_animationCompleteDelegate( this );
		}
	}
	
	public Vector2 anchorPointOnScreen() {
		return acnhorPointOnScreen;
	}
}
