using UnityEngine;
using System.Collections;

/// <summary>
/// Parent layout
/// </summary>
public class blindGUILayout {
	
	public readonly Vector2 realAnchorPoint;
	public readonly Vector2 anchorPoint;
	public readonly Vector2 scale;
	public readonly Vector2 size;
	public readonly float angle;
	
	public blindGUILayout( Vector2 _realAnchorPoint, Vector2 _anchorPoint, Vector2 _scale, Vector2 _size, float _angle ) {
		realAnchorPoint = _realAnchorPoint;
		anchorPoint = _anchorPoint;	
		scale = _scale;
		size = _size;
		angle = _angle;
	}
	
	override public string ToString() {
		return "Real anchor Point: "+realAnchorPoint.ToString()+
			"\n Relative anchor Point: "+anchorPoint.ToString()+
			"\n Scale: "+scale.ToString()+
			"\n Size: "+size.ToString()+
			"\n Angle: "+angle.ToString();
	}
}