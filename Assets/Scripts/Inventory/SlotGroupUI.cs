using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotGroupUI : MonoBehaviour
{
	[SerializeField]
	private GroupType _groupType;
	public GroupType GroupType {  get { return _groupType; } }

	[SerializeField]
	private Color _groupColor;

	[SerializeField]
	private SlotUI[] _slots;

	public int GroupSize { get { return _slots.Length; } }

	private void Awake()
	{
		Initialize();
	}


	private void Initialize()
	{
		if (_slots != null)
		{
			for (int i = 0; i < _slots.Length; i++)
			{
				if (_slots[i] != null)
				{
					_slots[i].SetColor(_groupColor);
					_slots[i].SetGroupType(_groupType);
				}
			}
		}
	}

	private void OnValidate()
	{
		Initialize();
	}


	/// <summary>
	/// Picks a random slot to start and cycles through attempting to find an undamaged slot
	/// </summary>
	/// <returns>True if successfull</returns>
	public bool DamageRandomSlot()
	{
		int index = UnityEngine.Random.Range(0, _slots.Length);

		for (int tries = 0; tries < _slots.Length; tries++)
		{
			if (!_slots[index].IsDamaged)
			{
				_slots[index].SetDamaged(true);

				return true;
			}

			index++;

			if (index >= _slots.Length)
			{
				index = 0;
			}
		}
		

		return false;
	}
}

public enum GroupType
{
	None,
	General,
	Weapon,
	Drive,
	Hat,
	Belt
}
