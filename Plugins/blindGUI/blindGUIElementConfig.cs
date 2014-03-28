using UnityEngine;
using System.Collections;

public class blindGUIElementConfig {
	public readonly Vector2 scale;
	public readonly Vector2 anchorPoint;
	public readonly Vector2 offset;
	public readonly Vector2 size;
	public readonly float angle;
	
	public blindGUIElementConfig( Vector2 _scale, Vector2 _anchorPoint, Vector2 _offset, Vector2 _size, float _angle ) {
		scale = _scale;
		anchorPoint = _anchorPoint;
		offset = _offset;
		size = _size;
		angle = _angle;
	}
	
}
