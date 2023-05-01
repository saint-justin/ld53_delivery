using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionUI : MonoBehaviour
{
	[SerializeField]
	private InventoryUI _inventoryUI;

	[SerializeField]
	private ActionButton _buttonPrefab;

	[SerializeField]
	private Transform _actionTab;


	public void PopulateActions(List<ItemUI> items)
	{

	}
}
