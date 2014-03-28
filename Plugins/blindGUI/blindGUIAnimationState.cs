using UnityEngine;
using System.Collections;

public class blindGUIAnimationState {
	
	public Vector2 size;
	public Vector2 anchorPoint;
	public Vector2 offset;
	public float   angle;
	public float   alpha;
	public float   scale;
	
	public blindGUIAnimationState( blindGUIParentElement element ) {
		size = element.m_size;
		scale = element.m_scale;
		offset = element.m_offset;
		angle = element.m_angle;
		alpha = element.m_alpha;
		scale = element.m_scale;
		anchorPoint = element.m_anchorPoint;
	}
	
	public blindGUIAnimationState() {
		size = new Vector2(100,100);
		scale = 1.0f;
		anchorPoint = new Vector2(0,0);
		offset = new Vector2(0,0);
		angle = 0;
		alpha = 1;
	}
	
	public blindGUIAnimationState( Vector2 _size, float _scale, Vector2 _anchorPoint, Vector2 _offset, float _angle, float _alpha ) {
		size = _size;
		scale = _scale;
		anchorPoint = _anchorPoint;
		offset = _offset;
		angle = _angle;
		alpha = _alpha;
	}
	
}
