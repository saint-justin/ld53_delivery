using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
	[SerializeField]
	private InventoryUI _inventoryUI;

	[SerializeField]
	private ShopButton _buttonPrefab;

	[SerializeField]
	private GameObject[] _shopTabs;

	[SerializeField]
	private Image[] _shopTabHighlights;

	[SerializeField]
	private Color _selectedColor;

	[SerializeField]
	private Color _unselectedColor;

	private Dictionary<GroupType, GameObject> _shopTabDict;


	private void Start()
	{
		_shopTabDict = new Dictionary<GroupType, GameObject>();
		_shopTabDict.Add(GroupType.General, _shopTabs[0]);
		_shopTabDict.Add(GroupType.Hat, _shopTabs[0]);
		_shopTabDict.Add(GroupType.Ammo, _shopTabs[0]);
		_shopTabDict.Add(GroupType.Water, _shopTabs[0]);
		_shopTabDict.Add(GroupType.Weapon, _shopTabs[1]);
		_shopTabDict.Add(GroupType.Drive, _shopTabs[2]);
		_shopTabDict.Add(GroupType.Belt, _shopTabs[3]);

		ItemSO[] itemSOs = _inventoryUI.ItemSOs;

		for (int i = 0; i < itemSOs.Length; i++)
		{
			if (_shopTabDict.TryGetValue(itemSOs[i].GroupType, out GameObject tab))
			{
				ShopButton button = Instantiate(_buttonPrefab, tab.transform, false);

				button.Initialize(itemSOs[i]);
			}
			else
			{
				Debug.Log("No tab for type: " + itemSOs[i].GroupType + " " + itemSOs[i].name);
			}

			
		}
	}


	public void SelectTab(int tabIndex)
	{
		for (int i = 0; i < _shopTabs.Length; i++)
		{
			if (i == tabIndex)
			{
				_shopTabs[i].SetActive(true);
				_shopTabHighlights[i].color = _selectedColor;
			}
			else
			{
				_shopTabs[i].SetActive(false);
				_shopTabHighlights[i].color = _unselectedColor;
			}
		}
	}
}
