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

	private bool _useable = true;

	private UnityEvent<int, ItemUI> _actionEvent;


	private void Start()
	{
		_useable = true;
		_background.color = _useableColor;
	}

	public void Initialize(ItemSO itemSO)
	{
		_itemSO = itemSO;

		_nameTMP.text = itemSO.Name + ":";

		_descriptionTMP.text = itemSO.Description;

		_icon.sprite = itemSO.Icon;

		_actionEvent = itemSO.ActionEvent;
	}


	public void OnPress()
	{
		Debug.Log("Action Pressed");

		_useable = !_useable;

		if (_useable)
		{
			_background.color = _useableColor;
		}
		else
		{
			_background.color = _unuseableColor;
		}

		if (_actionEvent != null)
		{
			_actionEvent.Invoke(0, _item);
		}
		
	}
}
