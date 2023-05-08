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

	private GameObject _messageBox;
	private TextMeshProUGUI _messageTMP;

	private ActionUI _parent;



	public void Initialize(ItemUI item, GameObject messageBox, TextMeshProUGUI messageTMP, ActionUI parent)
	{
		_parent = parent;

		_messageBox = messageBox;

		_messageTMP = messageTMP;

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
			_parent.SetAllBorderColors(Color.white);

			if (_itemSO.RequireTarget)
			{
				OnSelectAbility();
			}
			else
			{
				OnUseAbility();
			}
			
		}
	}


	private void OnSelectAbility()
	{
		EncounterManager.Instance.SetSelectedAbility(_itemSO.AbilityType, OnUseAbility);

		_messageTMP.text = "Select A Target";

		_border.color = Color.green;

		AudioManager.Instance.PlaySound(_itemSO.ChargingSound);
	}

	private void OnUseAbility()
	{
		if (_actionEvent != null)
		{
			//Debug.Log("Setting item to used");
			_item.IsUsed = true;
			_actionEvent.Invoke(0, _item);

			_messageTMP.text = _item.ItemSO.AbilityMessage;

			_border.color = Color.white;

			AudioManager.Instance.PlaySound(_itemSO.AbilitySound);
		}

		SetUseable(false);

		_parent.RefreshUseable();
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


	public void SetBorderColor(Color color)
	{
		_border.color = color;
	}


	public bool CheckUseable()
	{
		PlayerStats stats = EncounterManager.Instance.CurrentStats;

		bool canUse = true;

		if (_item.IsUsed)
		{
			canUse = false;
		}
		else if (_itemSO.Energy > stats.Energy)
		{
			canUse = false;
		}
		else if (_itemSO.Water > stats.Water)
		{
			canUse = false;
		}
		else if (_itemSO.Ammo > stats.Ammo)
		{
			canUse = false;
		}

		SetUseable(canUse);

		return canUse;
	}
}
