using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour, IPointerDownHandler
{
	public static InventoryUI Instance;

	public bool LockInventory { get; set; }

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
	public ItemSO[] ItemSOs { get { return _itemSOs; } }

	[SerializeField]
	private Transform _ptrRaycastDebug;

	[SerializeField]
	private SlotGroupUI[] _slotGroups;

	private Dictionary<int, ItemSO> _itemDict;

	private Dictionary<GroupType, SlotGroupUI> _slotGroupDict;

	private LinkedList<ItemUI> _items;

	private int _lastDamagedGroup;

	[SerializeField]
	private TextMeshProUGUI _scoreTMP;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}


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

		_items = new LinkedList<ItemUI>();


		_slotGroupDict = new Dictionary<GroupType, SlotGroupUI>();

		for (int i = 0; i < _slotGroups.Length; i++)
		{
			_slotGroupDict.Add(_slotGroups[i].GroupType, _slotGroups[i]);
		}
	}


	public void OnPointerDown(PointerEventData eventData)
	{
		GameObject hitObject = eventData.pointerCurrentRaycast.gameObject;

		if (LockInventory || hitObject == null)
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
			//Debug.Log("Found item with Slot tag");
			SlotUI slot = hitObject.GetComponent<SlotUI>();

			if (slot != null)
			{
				ItemUI item = _cursor.CurrentItem;

				if (_cursor.PlaceItem(slot))
				{
					AddItemToInventory(item);

					TallyScore();
				}
			}
			
		}
		else if (!_cursor.HasItem && hitObject.CompareTag("Item"))
		{
			BlockUI block = hitObject.GetComponent<BlockUI>();

			if (block != null)
			{
				_cursor.PickItem(block.GetParentItem);

				RemoveItemFromInventory(block.GetParentItem);

				TallyScore();
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


	public void DeleteCursorItem()
	{
		_cursor.DeleteItem();
	}


	public void AddItemToInventory(ItemUI item)
	{
		if (item != null)
		{
			_items.AddLast(item);
		}
	}


	public void RemoveItemFromInventory(ItemUI item)
	{
		if (item != null)
		{
			if (_items.Remove(item))
			{
				//Debug.Log("Removed item");
			}
		}
	}


	public bool DamageSlotInGroup(GroupType groupType)
	{
		if (_slotGroupDict.TryGetValue(groupType, out SlotGroupUI slotGroup))
		{
			return slotGroup.DamageRandomSlot();
		}

		return false;
	}


	public float TallyScore()
	{
		float score = 0;

		foreach (ItemUI item in _items)
		{
			if (!item.IsDamaged && item.ItemSO.Value != 0)
			{
				if (item.IsTouchingGroupType(GroupType.Belt))
				{
					score += item.ItemSO.Value * 1.5f;
				}
				else
				{
					score += item.ItemSO.Value;
				}
			}
		}

		_scoreTMP.text = "$ " + score.ToString();

		return score;
	}


	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.L))
		{
			LockInventory = !LockInventory;
		}

		if (Input.GetKeyDown(KeyCode.P))
		{
			_lastDamagedGroup++;

			if (_lastDamagedGroup >= _slotGroups.Length)
			{
				_lastDamagedGroup = 0;
			}

			DamageSlotInGroup(_slotGroupDict[_slotGroups[_lastDamagedGroup].GroupType].GroupType);


			TallyScore();
		}
	}
}
