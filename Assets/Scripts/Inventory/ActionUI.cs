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

	[SerializeField]
	private List<ActionButton> _actions;


	public void PopulateActions(List<ItemUI> items)
	{
		for (int i = 0; i < _actions.Count; i++)
		{
			Destroy(_actions[i].gameObject);
		}

		_actions = new List<ActionButton>();

		for (int i = 0; i < items.Count; i++)
		{
			ActionButton button = Instantiate(_buttonPrefab, _actionTab, false);
			button.Initialize(items[i]);

			_actions.Add(button);
		}
	}
}
