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

	private void Start()
	{
		_messageTMP.text = "Select an Ability";
	}

	public void PopulateActions(List<ItemUI> items)
	{
		for (int i = 0; i < _actions.Count; i++)
		{
			Destroy(_actions[i].gameObject);
		}

		_actions = new List<ActionButton>();

		for (int i = 0; i < items.Count; i++)
		{
			if (!items[i].IsUsed && items[i].ItemSO.ActionEvent != null)
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

		RefreshUseable();
	}


	public void RefreshUseable()
	{
		for (int i = 0; i < _actions.Count; i++)
		{
			_actions[i].CheckUseable();
		}
	}


	public void SetAllBorderColors(Color color)
	{
		foreach (ActionButton button in _actions)
		{
			button.SetBorderColor(color);
		}
	}


	public void SetMessage(string message)
	{
		_messageTMP.text = message;
	}
}
