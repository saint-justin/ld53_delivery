using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour
{
	[SerializeField]
	private Image _icon;

	[SerializeField]
	private Image _background;

	[SerializeField]
	private Image _border;

	[SerializeField]
	private Button _button;

	[SerializeField]
	private TextMeshProUGUI _nameTMP;

	[SerializeField]
	private TextMeshProUGUI _descriptionTMP;

	[SerializeField]
	private Color _useableColor;

	[SerializeField]
	private Color _unuseableColor;


	private ItemSO _itemSO;

	private ItemUI _item;

	private bool _useable;

	private UnityEvent<int, ItemUI> _actionEvent;

	private bool _isUsed;


	public void Initialize(ItemUI item)
	{
		_item = item;
		_itemSO = item.ItemSO;

		_nameTMP.text = _itemSO.Name + ":";

		_descriptionTMP.text = _itemSO.Description;

		_icon.sprite = _itemSO.Icon;

		_actionEvent = _itemSO.ActionEvent;

		Debug.Log("Item was used" + _item.IsUsed);
		SetUseable(!_item.IsUsed);
	}


	public void OnPress()
	{
		if (_useable && !InventoryUI.Instance.ShieldPending)
		{
			if (_itemSO.RequireTarget)
			{
				EncounterManager.Instance.SetSelectedAbility(OnUseAbility);
			}
			else
			{
				OnUseAbility();
			}
			
		}
	}


	private void OnUseAbility()
	{
		if (_actionEvent != null)
		{
			//Debug.Log("Setting item to used");
			_item.IsUsed = true;
			_actionEvent.Invoke(0, _item);
		}

		SetUseable(false);
	}


	private void SetUseable(bool useable)
	{
		Debug.Log("Set button to useable: " + useable);

		_useable = useable;

		if (_useable)
		{
			_background.color = _useableColor;
		}
		else
		{
			_background.color = _unuseableColor;
		}
	}
}
