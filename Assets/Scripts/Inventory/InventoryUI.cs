using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour, IPointerDownHandler
{
	public static InventoryUI Instance;

	public bool LockInventory { get; set; }
	public bool LockBelt { get; set; }
	public bool ShieldPending { get; set; }

	[SerializeField]
	private CursorUI _cursor;
	public CursorUI Cursor { get { return _cursor; } }

	[SerializeField]
	private Transform _itemParent;
	public Transform ItemParent { get { return _itemParent; } }

	private EventSystem _eventSystem;

	[SerializeField]
	private GraphicRaycaster _graphicRaycaster;
	public GraphicRaycaster GraphicRaycaster { get { return _graphicRaycaster; } }

	[SerializeField]
	private ItemSO[] _itemSOs;
	public ItemSO[] ItemSOs { get { return _itemSOs; } }

	[SerializeField]
	private Transform _ptrRaycastDebug;

	[SerializeField]
	private SlotGroupUI[] _slotGroups;

	[SerializeField]
	private Shield[] _shieldPrefabs;

	private Dictionary<int, ItemSO> _itemDict;

	private Dictionary<GroupType, SlotGroupUI> _slotGroupDict;

	private List<ItemUI> _items;

	private int _lastDamagedGroup;

	private Transform _followTarget;

	private Shield _pendingShield;

	[SerializeField]
	private TextMeshProUGUI _scoreTMP;

	[SerializeField]
	private ShopUI _shop;

	[SerializeField]
	private ActionUI _actionBar;

	[SerializeField]
	private Transform _rearrange;

	[SerializeField]
	private DamagePlacement _damagePlacement;


	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;

			DontDestroyOnLoad(transform.parent);

			SetInventoryState(InventoryState.Load);
		}
		else
		{
			Debug.LogError("Tried to load an additional Inventory");
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

		_items = new List<ItemUI>();


		_slotGroupDict = new Dictionary<GroupType, SlotGroupUI>();

		for (int i = 0; i < _slotGroups.Length; i++)
		{
			_slotGroupDict.Add(_slotGroups[i].GroupType, _slotGroups[i]);
		}
	}


	public void SetFollowTarget(Transform target)
	{
		_followTarget = target;
	}


	public void OnPointerDown(PointerEventData eventData)
	{
		GameObject hitObject = eventData.pointerCurrentRaycast.gameObject;

		if (hitObject == null)
		{
			return;
		}

		SlotUI slot = hitObject.GetComponent<SlotUI>();
		BlockUI block = hitObject.GetComponent<BlockUI>();
		Shield shield = hitObject.GetComponent<Shield>();

		if (shield != null)
		{
			Debug.Log("Clicked Shield");
		}


		if (eventData.button == PointerEventData.InputButton.Right)
		{
			if (slot != null)
			{
				slot.SetDamaged(!slot.IsDamaged);
			}

			return;
		}


		if (ShieldPending)
		{
			if (slot != null)
			{
				PlaceShield(slot.transform.position);
			}
			else if (block != null)
			{
				PlaceShield(block.transform.position);
			}

			return;
		}

		if (!LockInventory && _cursor.HasItem && slot != null)
		{
			ItemUI item = _cursor.CurrentItem;

			if (_cursor.PlaceItem(slot))
			{
				AddItemToInventory(item);

				TallyItemAttributes();
			}

		}
		else if (!LockInventory && !_cursor.HasItem && block != null)
		{
			ItemUI item = block.GetParentItem;

			if (!LockBelt || !item.IsTouchingGroupType(GroupType.Belt))
			{
				_cursor.PickItem(block.GetParentItem);

				RemoveItemFromInventory(block.GetParentItem);

				TallyItemAttributes();
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
			_items.Add(item);
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


	private bool DamageSlotInGroup(GroupType groupType)
	{
		if (_slotGroupDict.TryGetValue(groupType, out SlotGroupUI slotGroup))
		{
			return slotGroup.DamageRandomSlot();
		}

		return false;
	}


	public ItemTally TallyItemAttributes()
	{
		ItemTally tally = new ItemTally();

		foreach (ItemUI item in _items)
		{
			ItemSO so = item.ItemSO;

			if (!item.IsDamaged)
			{
				tally.Ammo += so.Ammo;
				tally.Energy += so.Energy;
				tally.Heat += so.Heat;
				tally.Water += so.Water;

				if (item.IsTouchingGroupType(GroupType.Belt))
				{
					tally.Value += (int)(1.5f * (float)item.ItemSO.Value);
				}
				else
				{
					tally.Value += item.ItemSO.Value;
				}
			}
		}

		_scoreTMP.text = "$ " + tally.Value.ToString();

		return tally;
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

			TallyItemAttributes();
		}

		if (_followTarget != null)
		{
			transform.position = _followTarget.position;
		}
	}


	public void SetInventoryState(InventoryState state)
	{
		switch (state)
		{
			case InventoryState.Hidden:
			{
				gameObject.SetActive(false);
				UnityEngine.Cursor.visible = true;
				break;
			}
			case InventoryState.Load:
			{
				gameObject.SetActive(true);
				UnityEngine.Cursor.visible = false;
				_shop.gameObject.SetActive(true);
				_actionBar.gameObject.SetActive(false);
				_rearrange.gameObject.SetActive(false);
				_damagePlacement.gameObject.SetActive(false);
				LockInventory = false;
				LockBelt = false;
				break;
			}
			case InventoryState.Encounter:
			{
				gameObject.SetActive(true);
				UnityEngine.Cursor.visible = false;
				_shop.gameObject.SetActive(false);
				_actionBar.gameObject.SetActive(true);
				_rearrange.gameObject.SetActive(false);
				_damagePlacement.gameObject.SetActive(true);
				LockInventory = true;
				LockBelt = true;
				break;
			}
			case InventoryState.Rearrange:
			{
				gameObject.SetActive(true);
				UnityEngine.Cursor.visible = false;
				_shop.gameObject.SetActive(false);
				_actionBar.gameObject.SetActive(false);
				_rearrange.gameObject.SetActive(true);
				_damagePlacement.gameObject.SetActive(false);
				LockInventory = false;
				LockBelt = true;
				break;
			}
			case InventoryState.Transition:
			{
				gameObject.SetActive(true);
				UnityEngine.Cursor.visible = false;
				_shop.gameObject.SetActive(false);
				_actionBar.gameObject.SetActive(false);
				_rearrange.gameObject.SetActive(false);
				_damagePlacement.gameObject.SetActive(false);
				LockInventory = true;
				LockBelt = true;
				break;
			}
			default:
			{
				break;
			}
		}
	}


	public void PopulateActions()
	{
		_actionBar.PopulateActions(_items);
	}


	public void ResetUsedItems()
	{
		for (int i = 0; i < _items.Count; i++)
		{
			_items[i].IsUsed = false;
		}
	}


	public void StartEncounter() {
		PopulateActions();
		if (SceneManager.GetActiveScene().name == "InventoryTestScene") {
			SceneManager.LoadScene("Encounter");
		} else {
			EncounterManager.Instance.ChangeState();
		}
	}

	public void FinishLoading()
	{
		if (EncounterManager.Instance != null)
		{
			EncounterManager.Instance.LoadEncounter(0);
		}
		else
		{
			SetInventoryState(InventoryState.Encounter);
		}

		PopulateActions();
	}

	public void FinishTurn()
	{
		EncounterManager.Instance.EndEncounter();
	}

	public void FinishRearange()
	{
		EncounterManager.Instance.NextEncounter();
	}


	public void PlaceChallengeDamage(DamagePattern[] challenge)
	{
		_damagePlacement.SetDamagePatterns(challenge);
	}


	public void ApplyDamage()
	{
		_damagePlacement.ApplyDamage(true, true);
	}


	public void MoveSpaces(int direction, int spaces)
	{
		_damagePlacement.MovePattern(direction, spaces);
	}


	public void GenerateShield(int abilityValue)
	{
		if (_pendingShield == null && abilityValue < _shieldPrefabs.Length)
		{
			_pendingShield = Instantiate(_shieldPrefabs[abilityValue], _itemParent, false);
			_pendingShield.Initialize();
			_damagePlacement.AddShield(_pendingShield);

			ShieldPending = true;
		}
	}


	public void PlaceShield(Vector3 position)
	{
		if (_pendingShield != null)
		{
			_pendingShield.PlaceShield(position);

			_pendingShield = null;
		}

		ShieldPending = false;
	}


	public void CancelShield()
	{
		if (_pendingShield != null)
		{
			Destroy(_pendingShield);

			_pendingShield = null;

			ShieldPending = false;
		}
	}
}


public enum InventoryState
{
	None,
	Load,
	Encounter,
	Hidden,
	Rearrange,
	Transition
}


public struct ItemTally
{
	public int Energy;
	public int Heat;
	public int Water;
	public int Ammo;
	public int Value;
}