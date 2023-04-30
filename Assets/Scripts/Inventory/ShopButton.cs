using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
	[SerializeField]
	private Image _image;

	[SerializeField]
	private Button _button;

	[SerializeField]
	private TextMeshProUGUI _nameTMP;

	[SerializeField]
	private TextMeshProUGUI _descriptionTMP;


	private ItemSO _itemSO;

	
	public void Initialize(ItemSO itemSO)
	{
		_itemSO = itemSO;

		_nameTMP.text = itemSO.Name + ":";

		_descriptionTMP.text = itemSO.Description;

		_image.sprite = itemSO.Icon;
	}


	public void OnPress()
	{
		InventoryUI.Instance.SpawnItem(_itemSO.ItemID);
	}
}
