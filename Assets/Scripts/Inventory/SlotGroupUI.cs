using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotGroupUI : MonoBehaviour
{
	[SerializeField]
	private GroupType _groupType;

	[SerializeField]
	private Color _groupColor;

	[SerializeField]
	private SlotUI[] _slots;

	


	private void OnValidate()
	{
		if (_slots != null)
		{
			for (int i = 0; i < _slots.Length; i++)
			{
				if (_slots[i] != null)
				{
					_slots[i].SetColor(_groupColor);
					_slots[i].SetGroupType(_groupType);
				}
			}
		}
	}
}

public enum GroupType
{
	None,
	General,
	Weapon,
	Drive,
	Hat,
	Belt
}
