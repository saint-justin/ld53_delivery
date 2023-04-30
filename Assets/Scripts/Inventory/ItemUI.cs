using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
	[SerializeField]
	private BlockUI[] _blocks;

	[SerializeField]
	private Image _icon;

	private ItemSO _itemSO;
	public ItemSO ItemSO { get { return _itemSO; } }

	private bool _isDamaged;
	public bool IsDamaged { get { return _isDamaged; } }

	private InventoryUI _inventoryUI;

	private GraphicRaycaster _rayCaster;

	private EventSystem _eventSystem;

	private PointerEventData _ptrEventData;

	private SlotUI[] _slots;

	public void Initialize(ItemSO itemSO, InventoryUI inventoryUI, GraphicRaycaster raycaster, EventSystem eventSystem)
	{
		_itemSO = itemSO;

		_icon.sprite = itemSO.Icon;

		_inventoryUI = inventoryUI;

		_rayCaster = raycaster;

		_eventSystem = eventSystem;

		_ptrEventData = new PointerEventData(_eventSystem);

		_slots = new SlotUI[_blocks.Length];

		_isDamaged = false;
	}


	public void SetColor(Color color)
	{
		_icon.color = color;
	}


	public bool CheckPlacement(out bool onGrid, out Vector3 snapPosition)
	{
		onGrid = false;
		snapPosition = Vector3.zero;

		bool valid = true;
		bool foundTypeMatch = false;

		for (int i = 0; i < _blocks.Length; i++)
		{
			List<RaycastResult> hits = new List<RaycastResult>();

			_ptrEventData.position = _blocks[i].transform.position;

			_rayCaster.Raycast(_ptrEventData, hits);

			SlotUI slot = null;

			for (int hit = 0; hit < hits.Count; hit++)
			{
				if (hits[hit].gameObject != null && hits[hit].gameObject.CompareTag("Slot"))
				{
					slot = hits[hit].gameObject.GetComponent<SlotUI>();
				}
			}

			if (slot != null)
			{
				onGrid = true;
				snapPosition = slot.transform.position + transform.position - _blocks[i].transform.position;

				//_blocks[i]._raycastDebug.position = snapPosition;

				if (!slot.HasItem && !slot.IsDamaged)
				{
					if (_itemSO.GroupType == GroupType.General || _itemSO.GroupType == slot.GroupType)
					{
						foundTypeMatch = true;
					}
					//Debug.Log($"Found valid slot at block {i}: {slot.name}");
				}
				else
				{
					valid = false;
					//Debug.Log($"Found invalid slot at block {i}: {slot.name}");
				}
			}
			else
			{
				valid = false;
				//Debug.Log($"Failed to find slot at block {i}");
			}
		}


		return valid && foundTypeMatch;
	}


	public void PickItem()
	{
		for (int i = 0; i < _blocks.Length; i++)
		{
			_blocks[i].EnableRayCast(false);
		}

		for (int i = 0; i < _slots.Length; i++)
		{
			if (_slots[i] != null)
			{
				_slots[i].PickItem();
				_slots[i] = null;
			}
		}
	}

	public void PlaceItem(Vector3 position)
	{
		for (int i = 0; i < _blocks.Length; i++)
		{
			List<RaycastResult> hits = new List<RaycastResult>();

			_ptrEventData.position = _blocks[i].transform.position;

			_rayCaster.Raycast(_ptrEventData, hits);

			for (int hit = 0; hit < hits.Count; hit++)
			{
				if (hits[hit].gameObject != null && hits[hit].gameObject.CompareTag("Slot"))
				{
					SlotUI slot = hits[hit].gameObject.GetComponent<SlotUI>();

					if (slot != null)
					{
						slot.PlaceItem(_blocks[i]);
						_slots[i] = slot;
					}
				}
			}

			_blocks[i].EnableRayCast(true);
		}


		transform.SetParent(_inventoryUI.ItemParent);

		transform.position = position;
	}


	public void SetRaycastDebug(Vector3 position)
	{
		for (int i = 0; i < _blocks.Length; i++)
		{
			_blocks[i]._raycastDebug.position = position;
		}
	}


	public bool IsTouchingGroupType(GroupType groupType)
	{
		for (int i = 0; i < _slots.Length; i++)
		{
			if (_slots[i] != null && _slots[i].GroupType == groupType)
			{
				return true;
			}
		}

		return false;
	}


	public void CheckDamage()
	{
		for (int i = 0; i < _blocks.Length; i++)
		{
			if (_blocks[i].IsDamaged)
			{
				_isDamaged = true;
				return;
			}
		}

		_isDamaged = false;
	}
}
