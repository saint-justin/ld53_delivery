using System.Collections;
using System.Collections.Generic;
using TMPro;
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

	[SerializeField]
	private GameObject _messageBox;

	[SerializeField]
	private TextMeshProUGUI _messageTMP;


	public void PopulateActions(List<ItemUI> items)
	{
		for (int i = 0; i < _actions.Count; i++)
		{
			Destroy(_actions[i].gameObject);
		}

		_actions = new List<ActionButton>();

		for (int i = 0; i < items.Count; i++)
		{
			

			if (items[i].ItemSO.ActionEvent != null)
			{
				string funcName = items[i].ItemSO.ActionEvent.GetPersistentMethodName(0);

				if (funcName != null && funcName != "")
				{
					ActionButton button = Instantiate(_buttonPrefab, _actionTab, false);
					button.Initialize(items[i], _messageBox, _messageTMP, this);

					_actions.Add(button);
				}
			}
		}
	}


	public void SetAllBorderColors(Color color)
	{
		foreach (ActionButton button in _actions)
		{
			button.SetBorderColor(color);
		}
	}
}
