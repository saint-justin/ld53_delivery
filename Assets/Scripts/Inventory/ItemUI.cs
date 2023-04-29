using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
	[SerializeField]
	private RectTransform[] _blocks;

	private Image _image;

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

		_image = GetComponent<Image>();

		_slots = new SlotUI[_blocks.Length];
	}


	public void SetColor(Color color)
	{
		_image.color = color;
	}


	public bool CheckPlacement()
	{
		for (int i = 0; i < _blocks.Length; i++)
		{
			List<RaycastResult> hits = new List<RaycastResult>();

			_ptrEventData.position = _blocks[i].position;

			_rayCaster.Raycast(_ptrEventData, hits);

			bool foundSlot = false;

			for (int hit = 0; hit < hits.Count; hit++)
			{
				if (hits[hit].gameObject != null && hits[hit].gameObject.CompareTag("Slot"))
				{
					SlotUI slot = hits[hit].gameObject.GetComponent<SlotUI>();

					if (slot != null && !slot.HasItem)
					{
						foundSlot = true;
					}
					else
					{
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
		_image.raycastTarget = false;

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
		transform.SetParent(_inventoryUI.ItemParent);

		//transform.position = position;

		_image.raycastTarget = true;



		for (int i = 0; i < _blocks.Length; i++)
		{
			List<RaycastResult> hits = new List<RaycastResult>();

			_ptrEventData.position = _blocks[i].position;

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
		}
	}
}
