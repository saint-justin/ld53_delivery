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

	private BlockUI _block;

	


	private void Start()
	{
		SetDamaged(false);
	}


	public void PlaceItem(BlockUI block)
	{
		_block = block;
		_item = block.GetParentItem;

		//Debug.Log("Placed item in slot");
	}


	public void PickItem()
	{
		if (_block.IsDamaged)
		{
			_block.SetDamaged(false);
		}

		_item = null;
		_block = null;

		//Debug.Log("Removed item in slot");
	}


	public void SetDamaged(bool damaged)
	{
		_isDamaged = damaged;

		_damageIcon.enabled = damaged;

		if (_block != null)
		{
			_block.SetDamaged(damaged);
		}
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
