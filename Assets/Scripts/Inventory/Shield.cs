using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Shield : MonoBehaviour
{
	public bool Placed { get; set; }

	[SerializeField]
	private Image _raycastTarget;

	private Transform _followTarget;


	public void Initialize()
	{
		Placed = false;

		_followTarget = InventoryUI.Instance.Cursor.transform;
	}


	public void EnableRaycastTarget(bool enable)
	{
		_raycastTarget.raycastTarget = enable;
	}


	private void Update()
	{
		if (!Placed && _followTarget != null)
		{
			transform.position = _followTarget.position;
		}
	}


	public void PlaceShield(Vector3 position)
	{
		transform.position = position;
		Placed = true;
	}
}
