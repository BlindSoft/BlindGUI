using UnityEngine;
using System.Collections;


public class blindGUIText : blindGUIParentElement {
	
	public string m_text = "";
	public Font   m_font = null;
	public Color  m_fontColor = Color.white;
	
	public TextAnchor m_textAnchor = TextAnchor.UpperLeft;
	
	public override void Draw (blindGUILayout parentLayout, bool active)
	{
		if (m_font == null) m_font = new Font();
		
		Rect elementFrame = GetFrame(parentLayout);
		
		m_style.font = m_font;
		m_style.normal.textColor = m_fontColor;
		m_style.alignment = m_textAnchor;
		
		GUI.Label(elementFrame,m_text,m_style);
		
		base.Draw (parentLayout, active);
	}
	
}
