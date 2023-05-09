using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamagePlacement : MonoBehaviour
{
	[SerializeField]
	private Transform _followTarget;

	[SerializeField]
	private int _maxPlaceAttempts;

	private DamagePattern[] _patterns;

	[SerializeField]
	private Canvas _canvas;

	[SerializeField]
	private GraphicRaycaster _raycaster;

	private Vector3 _offset;

	private float _slotWidth;

	public List<Shield> _shields;

	public bool PendingPlacement;


	private void Start()
	{
		Initialize();
	}


	public void Initialize()
	{
		_slotWidth = 30f;

		_offset = Vector3.zero;

		if (_shields != null)
		{
			for (int i = 0; i < _shields.Count; i++)
			{
				Destroy(_shields[i]);
			}
		}

		_shields = new List<Shield>();
	}


	public void AddShield(Shield shield)
	{
		_shields.Add(shield);
	}


	public void ClearShields()
	{
		if (_shields != null)
		{
			for (int i = _shields.Count - 1; i >= 0; i--)
			{
				Destroy(_shields[i].gameObject);
				_shields.RemoveAt(i);
			}
		}
	}


	public void ClearPatterns()
	{
		_offset = Vector3.zero;

		if (_patterns != null)
		{
			for (int i = 0; i < _patterns.Length; i++)
			{
				Destroy(_patterns[i].gameObject);
			}

			_patterns = null;
		}
	}


	public void SetDamagePatterns(DamagePattern[] damagePatterns, Vector2 pos, bool visible, bool avoidDamaged, bool asHeat)
	{
		ClearPatterns();

		if (damagePatterns == null)
		{
			return;
		}

		_patterns = new DamagePattern[damagePatterns.Length];

		for (int i = 0; i < _patterns.Length; i++)
		{
			_patterns[i] = Instantiate(damagePatterns[i], transform, false);

			_patterns[i].Initialize(_raycaster, _canvas, _maxPlaceAttempts, visible);
		}

		Debug.Log($"{gameObject.name} active: {gameObject.activeInHierarchy}");

		//PlaceDamagePatterns(pos, avoidDamaged, asHeat);

		//if (applyImmediate)
		//{
		//	ApplyDamage(true);
		//}

		//StartCoroutine(DelayedPlace(pos, avoidDamaged, asHeat, applyImmediate));
	}


	public void ApplyDamage(bool damage)
	{
		EnableShieldRaycast(true);

		if (_patterns == null)
		{
			Debug.LogError("Damage Pattern not set");
			return;
		}

		for (int i = 0; i < _patterns.Length; i++)
		{
			_patterns[i].ApplyDamage(damage);
		}

		InventoryUI.Instance.TallyItemAttributes();

		EnableShieldRaycast(false);
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


	public void PlaceDamagePatterns(Vector2 pos, bool avoidDamaged, bool asHeat)
	{
		if (pos != Vector2.zero)
		{
			_patterns[0].SetPosition(pos, transform.position);
			//Debug.Log("Placed By Position");
			return;
		}

		//Debug.Log("Placed By Random");

		EnableRaycastTarget(true);

		for (int i = 0; i < _patterns.Length; i++)
		{
			if (i > 0)
			{
				//Debug.Log("Raycast enabled " + _patterns[i - 1].IsRaycastEnabled());
			}

			if (!_patterns[i].FindValidSpot(transform.position, avoidDamaged, asHeat))
			{
				//Debug.LogWarning("PlaceDamagePatterns failed to find a valid location for damage");
				_patterns[i].gameObject.SetActive(false);
			}
		}

		//EnableRaycastTarget(false);
	}


	public void EnableRaycastTarget(bool enable)
	{
		for (int i = 0; i < _patterns.Length; i++)
		{
			_patterns[i].EnableRaycastTarget(enable);

			//Debug.Log($"Set {_patterns[i].gameObject.name} RaycastEnable to: {enable}");
		}
	}


	private void Update()
	{
		if ( Input.GetKeyDown(KeyCode.O))
		{
			ApplyDamage(true);
		}

		if (Input.GetKeyDown(KeyCode.P))
		{
			PlaceDamagePatterns(Vector2.zero, true, false);
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


	private void EnableShieldRaycast(bool enable)
	{
		if (_shields != null)
		{
			for (int i = 0; i < _shields.Count; i++)
			{
				_shields[i].EnableRaycastTarget(enable);
			}
		}
	}


	private IEnumerator DelayedPlace(Vector2 pos, bool avoidDamaged, bool asHeat, bool applyImmediate)
	{
		gameObject.SetActive(false);

		yield return new WaitForSeconds(0.04f);

		gameObject.SetActive(true);

		PlaceDamagePatterns(pos, avoidDamaged, asHeat);

		if (applyImmediate)
		{
			ApplyDamage(true);
		}
	}


	public void SetVisible(bool visible)
	{
		foreach(DamagePattern pattern in _patterns)
		{
			pattern.SetVisible(visible);
		}
	}
}
