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
	private ItemSO[] _itemSOs;

	[SerializeField]
	private Transform _ptrRaycastDebug;

	private Dictionary<int, ItemSO> _itemDict;


	private void Start()
	{
		_itemDict = new Dictionary<int, ItemSO>();

		for (int i = 0; i < _itemSOs.Length; i++)
		{
			if (_itemDict.ContainsKey(_itemSOs[i].ItemID))
			{
				_itemDict[_itemSOs[i].ItemID] = _itemSOs[i];
			}
			else
			{
				_itemDict.Add(_itemSOs[i].ItemID, _itemSOs[i]);
			}
		}
	}


	public void OnPointerDown(PointerEventData eventData)
	{
		GameObject hitObject = eventData.pointerCurrentRaycast.gameObject;

		if (hitObject == null)
		{
			return;
		}

		if (_ptrRaycastDebug != null)
		{
			_ptrRaycastDebug.position = eventData.position;
		}
		

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
		if (!_cursor.HasItem && _itemDict.TryGetValue(itemID, out ItemSO itemSO))
		{
			if (itemSO.Prefab == null)
			{
				Debug.LogWarning($"Failed to spawn item: {itemSO} Check the ItemSO for missing Prefab");
				return;
			}

			ItemUI item = Instantiate(itemSO.Prefab, _itemParent, false);

			item.Initialize(itemSO, this, _graphicRaycaster, _eventSystem);

			item.transform.position = _cursor.transform.position;

			_cursor.PickItem(item);
		}
	}


	public void DeleteItem()
	{
		_cursor.DeleteItem();
	}
}
