using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour, IPointerDownHandler
{
	[SerializeField]
	private CursorUI _cursor;

	[SerializeField]
	private Transform _itemParent;
	public Transform ItemParent { get { return _itemParent; } }



	[SerializeField]
	private EventSystem _eventSystem;

	[SerializeField]
	private GraphicRaycaster _graphicRaycaster;

	[SerializeField]
	private ItemUI[] _itemPrefabs;


	public Transform Blip;


	public void OnPointerDown(PointerEventData eventData)
	{
		GameObject hitObject = eventData.pointerCurrentRaycast.gameObject;

		if (hitObject == null)
		{
			return;
		}


		//Blip.position = eventData.position;


		if (eventData.button == PointerEventData.InputButton.Right)
		{
			SlotUI slot = hitObject.GetComponent<SlotUI>();

			if (slot != null)
			{
				slot.SetDamaged(!slot.IsDamaged);
			}

			return;
		}


		if (_cursor.HasItem && hitObject.CompareTag("Slot"))
		{
			Debug.Log("Found item with Slot tag");
			SlotUI slot = hitObject.GetComponent<SlotUI>();

			if (slot != null)
			{
				_cursor.PlaceItem(slot);
			}
			
		}
		else if (!_cursor.HasItem && hitObject.CompareTag("Item"))
		{
			BlockUI block = hitObject.GetComponent<BlockUI>();

			if (block != null)
			{
				_cursor.PickItem(block.GetParentItem);
			}
		}
	}


	public void SpawnItem(int itemID)
	{
		if (!_cursor.HasItem)
		{
			ItemUI item = Instantiate(_itemPrefabs[itemID], _itemParent, false);

			item.Initialize(this, _graphicRaycaster, _eventSystem);

			item.transform.position = _cursor.transform.position;

			_cursor.PickItem(item);
		}
	}


	public void DeleteItem()
	{
		_cursor.DeleteItem();
	}
}
