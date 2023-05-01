using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageGroup : MonoBehaviour
{
	[SerializeField]
	private Transform _followTarget;

	[SerializeField]
	private DamagePattern[] _patterns;

	[SerializeField]
	private Canvas _canvas;

	private Vector3 _offset;

	private float _slotWidth = 30f;


	private void Start()
	{
		GraphicRaycaster raycaster = InventoryUI.Instance.GraphicRaycaster;

		for (int i = 0; i < _patterns.Length; i++)
		{
			_patterns[i].Initialize(raycaster);
		}

		_offset = Vector3.zero;
	}


	public void ApplyDamage(bool damage, bool hidePattern = true)
	{
		for (int i = 0; i < _patterns.Length; i++)
		{
			_patterns[i].ApplyDamage(damage);
		}

		gameObject.SetActive(!hidePattern);

		InventoryUI.Instance.TallyScore();
	}


	/// <summary>
	/// Move in the given direction (0 = Up, 1 = Left, 2 = Down, 3 = Right)
	/// </summary>
	/// <param name="direction"></param>
	/// <param name="spaces"></param>
	public void MovePattern(int direction, int spaces)
	{
		switch (direction)
		{
			case 0:
			{
				_offset.y += spaces * _slotWidth * _canvas.scaleFactor;
				break;
			}
			case 1:
			{
				_offset.x -= spaces * _slotWidth * _canvas.scaleFactor;
				break;
			}
			case 2:
			{
				_offset.y -= spaces * _slotWidth * _canvas.scaleFactor;
				break;
			}
			case 3:
			{
				_offset.x += spaces * _slotWidth * _canvas.scaleFactor;
				break;
			}
		}
	}


	private void Update()
	{
		if ( Input.GetKeyDown(KeyCode.P))
		{
			ApplyDamage(true, true);


		}

		if (Input.GetKeyDown(KeyCode.W))
		{
			MovePattern(0, 1);
		}
		else if (Input.GetKeyDown(KeyCode.A))
		{
			MovePattern(1, 1);
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			MovePattern(2, 1);
		}
		else if (Input.GetKeyDown(KeyCode.D))
		{
			MovePattern(3, 1);
		}

		transform.position = _followTarget.position + _offset;
	}
}
