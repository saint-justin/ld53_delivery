using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour
{
	[SerializeField]
	private Image _slotIcon;

	[SerializeField]
	private Image _damageIcon;


	public bool HasItem { get { return _item != null; } }

	private bool _isDamaged;
	public bool IsDamaged { get { return _isDamaged; } }

	private GroupType _groupType;
	public GroupType GroupType { get { return _groupType; } }

	private ItemUI _item;

	


	private void Start()
	{
		SetDamaged(false);
	}


	public void PlaceItem(ItemUI item)
	{
		_item = item;

		Debug.Log("Placed item in slot");
	}


	public void PickItem()
	{
		_item = null;

		Debug.Log("Removed item in slot");
	}


	public void SetDamaged(bool damaged)
	{
		_isDamaged = damaged;

		_damageIcon.enabled = damaged;
	}


	public void SetColor(Color color)
	{
		_slotIcon.color = color;
	}


	public void SetGroupType(GroupType groupType)
	{
		_groupType = groupType;
	}
}
