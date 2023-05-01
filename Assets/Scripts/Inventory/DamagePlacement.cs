using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePlacement : MonoBehaviour
{
	[SerializeField]
	private Transform _followTarget;

	private DamagePattern[] _patterns;

	[SerializeField]
	private Canvas _canvas;

	[SerializeField]
	private GraphicRaycaster _raycaster;

	private Vector3 _offset;

	private float _slotWidth = 30f;


	private void Start()
	{
		_offset = Vector3.zero;
	}


	public void SetDamagePatterns(DamagePattern[] damagePatterns)
	{
		if (damagePatterns == null)
		{
			Debug.LogWarning("Attempted to set empty damagePatternArray");
			return;
		}

		if (_patterns != null)
		{
			for (int i = 0; i < _patterns.Length; i++)
			{
				Destroy(_patterns[i]);
			}
		}

		_patterns = new DamagePattern[damagePatterns.Length];

		for (int i = 0; i < _patterns.Length; i++)
		{
			_patterns[i] = Instantiate(damagePatterns[i], transform, false);

			_patterns[i].Initialize(_raycaster, _canvas);
		}

		PlaceDamagePatterns();
	}


	public void ApplyDamage(bool damage, bool hidePattern = true)
	{
		for (int i = 0; i < _patterns.Length; i++)
		{
			_patterns[i].ApplyDamage(damage);
		}

		gameObject.SetActive(!hidePattern);

		InventoryUI.Instance.TallyItemAttributes();
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
				_offset.y -= spaces * _slotWidth * _canvas.scaleFactor;
				break;
			}
			case 1:
			{
				_offset.x += spaces * _slotWidth * _canvas.scaleFactor;
				break;
			}
			case 2:
			{
				_offset.y += spaces * _slotWidth * _canvas.scaleFactor;
				break;
			}
			case 3:
			{
				_offset.x -= spaces * _slotWidth * _canvas.scaleFactor;
				break;
			}
		}
	}


	public void PlaceDamagePatterns()
	{
		EnableRaycastTarget(true);

		for (int i = 0; i < _patterns.Length; i++)
		{
			if (!_patterns[i].FindValidSpot(transform.position))
			{
				Debug.LogWarning("PlaceDamagePatterns failed to find a valid location for damage");
			}
		}

		EnableRaycastTarget(false);
	}


	public void EnableRaycastTarget(bool enable)
	{
		for (int i = 0; i < _patterns.Length; i++)
		{
			_patterns[i].EnableRaycastTarget(enable);
		}
	}


	private void Update()
	{
		if ( Input.GetKeyDown(KeyCode.O))
		{
			ApplyDamage(true, true);
		}

		if (Input.GetKeyDown(KeyCode.P))
		{
			PlaceDamagePatterns();
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

		transform.position = _offset + _followTarget.position;
	}
}
