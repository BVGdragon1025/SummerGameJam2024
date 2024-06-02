using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTip : MonoBehaviour
{
	public string message;

	void OnMouseEnter()
	{
		ToolTipManager.Instance.SetAndShowToolTip(message);
	}


	void OnMouseExit()
	{
		ToolTipManager.Instance.HideToolTip();
	}
}