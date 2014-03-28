using UnityEngine;
using System.Collections;

public class blindGUITextArea : blindGUIParentElement {
	
	public string m_text = "";
	
	// BUG With Unicode Font
	public Font   m_font = null;
	public Color  m_fontColor = Color.white;
	public FontStyle m_fontStyle = FontStyle.Normal;
	public int m_maxLength = -1;
	
	public TextAnchor m_textAnchor = TextAnchor.UpperLeft;
	
	public override void Draw (blindGUILayout parentLayout, bool active)
	{
		if (m_font == null) m_font = new Font();
		
		Rect elementFrame = GetFrame(parentLayout);
		
		m_style.font = m_font;
		m_style.fontStyle = m_fontStyle;
		m_style.normal.textColor = m_fontColor;
		m_style.alignment = m_textAnchor;
		
		m_text = GUI.TextArea(elementFrame,m_text,m_maxLength,m_style);
		Debug.Log(m_text);
		
		base.Draw (parentLayout, active);
	}
	
}