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

	private InventoryUI _inventoryUI;

	private GraphicRaycaster _rayCaster;

	private EventSystem _eventSystem;

	private PointerEventData _ptrEventData;

	private SlotUI[] _slots;

	public void Initialize(InventoryUI inventoryUI, GraphicRaycaster raycaster, EventSystem eventSystem)
	{
		_inventoryUI = inventoryUI;

		_rayCaster = raycaster;

		_eventSystem = eventSystem;

		_ptrEventData = new PointerEventData(_eventSystem);

		_slots = new SlotUI[_blocks.Length];
	}


	public void SetColor(Color color)
	{
		_icon.color = color;
	}


	public bool CheckPlacement()
	{
		for (int i = 0; i < _blocks.Length; i++)
		{
			List<RaycastResult> hits = new List<RaycastResult>();

			_ptrEventData.position = _blocks[i].transform.position;

			_rayCaster.Raycast(_ptrEventData, hits);

			bool foundSlot = false;

			for (int hit = 0; hit < hits.Count; hit++)
			{
				if (hits[hit].gameObject != null && hits[hit].gameObject.CompareTag("Slot"))
				{
					SlotUI slot = hits[hit].gameObject.GetComponent<SlotUI>();

					if (slot != null && !slot.HasItem && !slot.IsDamaged)
					{
						foundSlot = true;
						Debug.Log($"Found valid slot at block {i}: {slot.name}");
					}
					else if(slot != null)
					{
						Debug.Log($"Found invalid slot at block {i}: {slot.name}");
						return false;
					}
					else
					{
						Debug.Log($"Failed to find slot at block {i}");
						return false;
					}
				}
			}

			if (!foundSlot)
			{
				return false;
			}
		}


		return true;
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

	public void PlaceItem()
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
						slot.PlaceItem(this);
						_slots[i] = slot;
					}
				}
			}

			_blocks[i].EnableRayCast(true);
		}


		transform.SetParent(_inventoryUI.ItemParent);


		List<RaycastResult> centerhits = new List<RaycastResult>();

		_ptrEventData.position = transform.position;

		_rayCaster.Raycast(_ptrEventData, centerhits);

		for (int hit = 0; hit < centerhits.Count; hit++)
		{
			if (centerhits[hit].gameObject != null && centerhits[hit].gameObject.CompareTag("Slot"))
			{
				SlotUI slot = centerhits[hit].gameObject.GetComponent<SlotUI>();

				if (slot != null)
				{
					transform.position = slot.transform.position;
				}
			}
		}
	}
}
